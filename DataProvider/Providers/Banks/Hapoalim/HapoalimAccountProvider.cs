using System;
using System.Collections.Generic;
using System.Linq;
using DataProvider.Providers.Banks.Hapoalim.Dto;
using DataProvider.Providers.Interfaces;
using DataProvider.Providers.Models;

namespace DataProvider.Providers.Banks.Hapoalim
{
    public class HapoalimAccountProvider : IBankAccountProvider
    {
        private readonly IHapoalimApi _api;
        private const string _providerName = "Hapoalim";

        public HapoalimAccountProvider(IHapoalimApi api)
        {
            _api = api;
        }
        
        public BankAccount GetAccount(BankAccountDescriptor accountDescriptor)
        {
            try
            {
                var accountDto = GenerateAccountByAccountId(accountDescriptor);
                if (accountDto == null)
                {
                    return null;
                }

                var account = GetAccountInfo(accountDto);

                DateTime now = DateTime.Now;
                var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                var transactions = GetAccountTransactions(accountDto, firstDayOfMonth, lastDayOfMonth);
                account.Transactions = transactions;

                return account;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public IEnumerable<BankAccount> GetAccounts()
        {
            IList<BankAccount> result = new List<BankAccount>();
            var accounts = _api.GetAccountsData();
            foreach (var account in accounts)
            {
                result.Add(GetAccountInfo(account));
            }
            return result;
        }

        public IEnumerable<Transaction> GetTransactions(BankAccountDescriptor accountDescriptor, DateTime startTime, DateTime endTime)
        {
            var account = GenerateAccountByAccountId(accountDescriptor);
            return GetAccountTransactions( account, startTime, endTime);
        }

        public IEnumerable<Loan> GetLoans(BankAccountDescriptor accountDescriptor)
        {
            var account = GenerateAccountByAccountId(accountDescriptor);
            return GetAccountLoans(account);
        }

        private HapoalimAccountResponse GenerateAccountByAccountId(BankAccountDescriptor accountDescriptor)
        {
            var accounts = _api.GetAccountsData();
            var account = accounts.FirstOrDefault(a => 
                a.AccountNumber.Equals(accountDescriptor?.AccountNumber, StringComparison.CurrentCultureIgnoreCase)
                && a.BranchNumber == accountDescriptor?.BranchNumber);

            return account;
        }

        private IList<Transaction> GetAccountTransactions(HapoalimAccountResponse accountDto,  DateTime startTime, DateTime endTime)
        {
            var transactions = _api.GetTransactions(accountDto, startTime, endTime);
            var result = new List<Transaction>();
            foreach (var transaction in transactions.Transactions)
            {
                var eventDate = new DateTime((int) (transaction.EventDate / 10000), (int) (transaction.EventDate / 100 % 100),
                    (int) (transaction.EventDate % 100)).AddMinutes((int) (transaction.ExpandedEventDate % 100));
                result.Add(new Transaction
                {
                    Id = (long)(transaction.ReferenceNumber + Math.Round(transaction.EventAmount) + Math.Round(transaction.CurrentBalance)),
                    PurchaseDate = eventDate,
                    PaymentDate = eventDate,
                    Description = transaction.ActivityDescription,
                    ProviderName = _providerName,
                    CurrentBalance = transaction.CurrentBalance,
                    Amount = transaction.EventAmount,
                    IsFee = transaction.ActivityTypeCode == (int)HapoalimActivityType.Fee,
                    Type = transaction.EventActivityTypeCode == 1 ? TransactionType.Income : TransactionType.Expense,
                    SupplierId = transaction.ReferenceNumber.ToString(),
                });
            }

            return result;
        }

        private IList<Loan> GetAccountLoans(HapoalimAccountResponse accountDto)
        {
            var loans = _api.GetMortgages(accountDto);
            var result = new List<Loan>();
            foreach (var loan in loans.Data)
            {
                var startDate = (loan.ExecutingDate == 0) ? DateTime.MinValue : new DateTime((int)(loan.ExecutingDate / 10000), (int)(loan.ExecutingDate / 100 % 100),
                    (int)(loan.ExecutingDate % 100)).AddMinutes((int)(loan.ExecutingDate % 100));

                var endDate = (loan.CalculatedEndDate == 0) ? DateTime.MinValue : new DateTime((int)(loan.CalculatedEndDate / 10000), (int)(loan.CalculatedEndDate / 100 % 100),
                    (int)(loan.CalculatedEndDate % 100)).AddMinutes((int)(loan.CalculatedEndDate % 100));
                

                var newLoan = new Loan
                {
                    LoanId = loan.MortgageLoanSerialId,

                    StartDate = startDate,
                    EndDate = endDate,

                    DeptAmount = loan.RevaluedBalance,
                    LastPaymentAmount = loan.PaymentAmount,
                    PrepaymentCommission = loan.PrepaymentCommissionTotalAmount,

                    InsuranceCompany = loan.LifeInsuranceCompanyName,
                    InterestType = loan.InterestTypeDescription,
                    LinkageType = loan.LinkageTypeDescription,
                };

                Double origAmount = 0;
                foreach (var subLoan in loan.SubLoanData)
                {
                    var sd = (subLoan.ExecutingDate == 0) ? DateTime.MinValue : new DateTime((int)(subLoan.ExecutingDate / 10000), (int)(subLoan.ExecutingDate / 100 % 100),
                        (int)(subLoan.ExecutingDate % 100)).AddMinutes((int)(subLoan.ExecutingDate % 100));

                    var ed = (subLoan.CalculatedEndDate == 0) ? DateTime.MinValue : new DateTime((int)(subLoan.CalculatedEndDate / 10000), (int)(subLoan.CalculatedEndDate / 100 % 100),
                        (int)(subLoan.CalculatedEndDate % 100)).AddMinutes((int)(subLoan.CalculatedEndDate % 100));

                    var ned = (subLoan.NextExitDate == 0) ? DateTime.MinValue : new DateTime((int)(subLoan.NextExitDate / 10000), (int)(subLoan.NextExitDate / 100 % 100),
                        (int)(subLoan.NextExitDate % 100)).AddMinutes((int)(subLoan.NextExitDate % 100));

                    origAmount += subLoan.SubLoansPrincipalAmount;
                    newLoan.SubLoans.Add(new Loan.SubLoan
                    {
                        Id = subLoan.SubLoansSerialId.ToString(),
                        OriginalAmount = subLoan.SubLoansPrincipalAmount,
                        StartDate = sd,
                        EndDate = ed,
                        NextExitDate = ned,
                        PrincipalAmount = subLoan.PrincipalBalanceAmount,
                        InterestAmount = subLoan.InterestAndLinkageTotalAmount,
                        DebtAmount = subLoan.PrincipalAndInterestAndInterestDeferredTotalAmount,
                        InterestRate = subLoan.ValidityInterestRate
                    });
                }

                newLoan.OriginalAmount = origAmount;
                result.Add(newLoan);
            }

            return result;
        }


        private BankAccount GetAccountInfo(HapoalimAccountResponse account)
        {
            var result = new BankAccount
            {
                Label = account.ProductLabel,
                AccountNumber = account.AccountNumber,
                BankNumber = account.BankNumber,
                BranchNumber = account.BranchNumber,
            };

            if (account.AccountClosingReasonCode > 0)
            {
                return result;
            }

            var balance = _api.GetBalance(account);
            result.Balance = balance?.CurrentBalance ?? 0;
            return result;
        }

        public void Dispose()
        {
            _api?.Dispose();
        }
    }

    public enum HapoalimActivityType
    {
        Fee = 473,
        Salary = 159,
        Loan = 469,
        Income = 175,
        DirectDebit = 515,
        Check = 493
    };
}
