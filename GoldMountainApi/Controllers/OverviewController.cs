using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using GoldMountainApi.Controllers.Helper;
using GoldMountainApi.Models;
using GoldMountainApi.Services;
using GoldMountainShared;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoldMountainApi.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    [Authorize]
    public class OverviewController : Controller
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly ICreditCardRepository _creditAccountRepository;
        private readonly IValidationHelper _validationHelper;

        public OverviewController(IBankAccountRepository bankAccountRepository, ICreditCardRepository creditAccountRepository,
            IValidationHelper validationHelper)
        {
            _bankAccountRepository = bankAccountRepository;
            _creditAccountRepository = creditAccountRepository;
            _validationHelper = validationHelper;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("user/{userId}/overview")]
        public async Task<OverviewDto> GetOverview(String userId)
        {
            if (!_validationHelper.ValidateUserPermissions(User, userId))
            {
                throw new AuthenticationException();
            }

            var expenses = new Dictionary<String, Decimal>(); //institution vs amount last 30 days
            var incomes = new Dictionary<String, Decimal>(); //institution vs amount last 30 days
            var cashFlowExpenses = new Dictionary<String, Decimal>(); //month vs amount last 6 months
            var cashFlowIncomes = new Dictionary<String, Decimal>(); //month vs amount last 6 months
            var institutions = new List<InstitutionOverviewDto>();
            var mortgageOverview = new LoanOverviewDto();
            var loanOverview = new LoanOverviewDto();
            int numberOfMortgages = 0;
            int numberOfLoans = 0;
            Decimal netWorth = 0;

            InitCashFlowDictionary(cashFlowExpenses);
            InitCashFlowDictionary(cashFlowIncomes);

            var bankAccounts = await _bankAccountRepository.GetAccountsByUserId(userId);
            foreach (var account in bankAccounts)
            {
                GenerateNetWorthFromBank(account, incomes, expenses);
                GenerateCashFlowFromBank(account, cashFlowIncomes, cashFlowExpenses);
                numberOfMortgages += GenerateMortgageFromBank(account, mortgageOverview);
                numberOfLoans += GenerateLoanFromBank(account, loanOverview);

                netWorth += account.Balance;

                institutions.Add(new InstitutionOverviewDto
                {
                    Label = account.Label,
                    ProviderName = account.ProviderName,
                    Balance = account.Balance
                });

            }

            var creditCards = await _creditAccountRepository.GetCardsByUserId(userId);
            foreach (var account in creditCards)
            {
                GenerateNetWorthFromCredit(account, incomes, expenses);
                netWorth += incomes[account.LastDigits] - expenses[account.LastDigits];

                institutions.Add(new InstitutionOverviewDto
                {
                    Label = account.LastDigits,
                    ProviderName = account.ProviderName,
                    Balance = incomes[account.LastDigits] - expenses[account.LastDigits]
                });
            }
            
            return new OverviewDto
            {
                NetWorth = netWorth,
                ListOfInstitutions = institutions,
                NetWorthIncomes = incomes,
                NetWorthExpenses = expenses,
                CashFlowIncomes = cashFlowIncomes,
                CashFlowExpenses = cashFlowExpenses,
                MortgageOverview = mortgageOverview,
                LoanOverview = loanOverview,
                NumberOfMortgages = numberOfMortgages,
                NumberOfLoans = numberOfLoans,
                Loans = new List<Decimal[]>(),
                Mortgages = new List<Decimal[]>()
            };
        }

        private int GenerateMortgageFromBank(BankAccountDoc account, LoanOverviewDto mortgageOverview)
        {
            decimal principal = 0;
            decimal interest = 0;
            decimal commission = 0;

            foreach (var mortgage in account.Mortgages)
            {
                principal += mortgage.DeptAmount;
                interest += mortgage.InterestAmount; 
                commission += mortgage.PrepaymentCommission;
            }

            mortgageOverview.Principal += principal;
            mortgageOverview.Interest += interest;
            mortgageOverview.Commission += commission;

            return account.Mortgages.Count();
        }

        private int GenerateLoanFromBank(BankAccountDoc account, LoanOverviewDto loanOverview)
        {
            decimal principal = 0;
            decimal interest = 0;

            foreach (var loan in account.Loans)
            {
                principal += loan.DeptAmount;
                interest += loan.DeptAmount * loan.InterestRate / 100 *
                            (loan.NumberOfInterestPayments - loan.NextInterestPayment + 1)/12;
            }

            loanOverview.Principal += principal;
            loanOverview.Interest += interest;
            return account.Loans.Count();
        }

        private void GenerateNetWorthFromCredit(CreditCardDoc account, Dictionary<string, Decimal> incomes, Dictionary<string, Decimal> expenses)
        {
            incomes.Add(account.LastDigits, 0);
            expenses.Add(account.LastDigits, 0);
            foreach (var transaction in account.Transactions.Where(t => t.PaymentDate.IsInThisMonth()))
            {
                if (transaction.Type == TransactionType.Income)
                {
                    incomes[account.LastDigits] += Math.Abs(transaction.Amount);
                }
                else if (transaction.Type == TransactionType.Expense)
                {
                    expenses[account.CardNumber] += Math.Abs(transaction.Amount);
                }
            }
        }

        private void GenerateCashFlowFromBank(BankAccountDoc account, Dictionary<string, Decimal> cashFlowIncomes, Dictionary<string, Decimal> cashFlowExpenses)
        {
            foreach (var transaction in account.Transactions.Where(t => t.PaymentDate.IsInLast6Months()))
            {
                var monthName = transaction.PaymentDate.ToString("MMM", CultureInfo.InvariantCulture);
                if (transaction.Type == TransactionType.Income)
                {
                    cashFlowIncomes[monthName] += Math.Abs(transaction.Amount);
                }
                else if (transaction.Type == TransactionType.Expense)
                {
                    cashFlowExpenses[monthName] += Math.Abs(transaction.Amount);
                }
            }
        }

        private void GenerateNetWorthFromBank(BankAccountDoc account, Dictionary<string, Decimal> incomes, Dictionary<string, Decimal> expenses)
        {
            incomes.Add(account.Label, 0);
            expenses.Add(account.Label, 0);
            foreach (var transaction in account.Transactions.Where(t => t.PaymentDate.IsInThisMonth()))
            {
                if (transaction.Type == TransactionType.Income)
                {
                    incomes[account.Label] += Math.Abs(transaction.Amount);
                }
                else if (transaction.Type == TransactionType.Expense)
                {
                    expenses[account.Label] += Math.Abs(transaction.Amount);
                }
            }
        }

        private void InitCashFlowDictionary(Dictionary<String, Decimal> dict)
        {
            var now = DateTime.Today;
            for (int i = 0; i < 7; i++)
            {
                var monthToAdd = now.AddMonths(-1 * i).ToString("MMM", CultureInfo.InvariantCulture);
                dict[monthToAdd] = 0;
            }
        }
    }
}