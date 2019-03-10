using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DataProvider.Providers.Banks.Hapoalim.Dto;
using DataProvider.Providers.Interfaces;
using DataProvider.Providers.Models;

namespace DataProvider.Providers.Banks.Hapoalim
{
    public class HapoalimAccountProvider : IBankAccountProvider
    {
        private readonly IHapoalimApi _api;
        private const string ProviderName = "Hapoalim";

        public HapoalimAccountProvider(IHapoalimApi api)
        {
            _api = api;
        }

        public IEnumerable<BankAccount> GetAccounts()
        {
            var hapoalimAccounts = _api.GetAccounts();
            var accounts = GenerateBankAccounts(hapoalimAccounts);
            return accounts;
        }

        public BankAccount GetAccount(BankAccountDescriptor accountDescriptor)
        {
            try
            {
                var id = BankAccount.GenerateAccountId(accountDescriptor.BankNumber, accountDescriptor.BankNumber,
                                                       accountDescriptor.AccountNumber);
                var account = GetAccount(id);
                if (account == null)
                {
                    return null;
                }

                DateTime now = DateTime.Now;
                var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                var transactions = GetAccountTransactions(account, firstDayOfMonth, lastDayOfMonth);
                account.Transactions = transactions;

                return account;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public IEnumerable<BankAccount> GetAccountsWithAllData(List<CreditCardDescriptor> creditCardDescriptor, DateTime startDate, DateTime endDate, bool includeDeatils = false)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BankTransaction> GetTransactions(BankAccountDescriptor accountDescriptor, DateTime startTime, DateTime endTime)
        {
            var id = BankAccount.GenerateAccountId(accountDescriptor.BankNumber, accountDescriptor.BankNumber,
                                                   accountDescriptor.AccountNumber);
            var account = GetAccount(id);
            return GetAccountTransactions( account, startTime, endTime);
        }

        public IEnumerable<Mortgage> GetMortgages(BankAccountDescriptor accountDescriptor)
        {
            var id = BankAccount.GenerateAccountId(accountDescriptor.BankNumber, accountDescriptor.BankNumber,
                accountDescriptor.AccountNumber);
            var account = GetAccount(id);
            return GetAccountMortgages(account);
        }

        public IEnumerable<Loan> GetLoans(BankAccountDescriptor accountDescriptor)
        {
            var id = BankAccount.GenerateAccountId(accountDescriptor.BankNumber, accountDescriptor.BankNumber,
                accountDescriptor.AccountNumber);
            var account = GetAccount(id);
            return GetAccountLoans(account);
        }

        #region Private
        private BankAccount GetAccount(string accountId)
        {
            var accounts = GetAccounts();
            var account = accounts.FirstOrDefault(a => a.Id == accountId);
            return account;
        }

        private List<BankAccount> GenerateBankAccounts(IEnumerable<HapoalimAccountResponse> response)
        {
            var result = new List<BankAccount>();
            foreach (var accountResponse in response)
            {
                var account = AutoMapper.Mapper.Map<BankAccount>(accountResponse);
                account.Id = BankAccount.GenerateAccountId(account.BankNumber, account.BranchNumber, account.AccountNumber);

                if (accountResponse.AccountClosingReasonCode == 0)
                {
                    try
                    {
                        var balance = _api.GetBalance(account);
                        account.Balance = balance;
                        result.Add(account);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
            return result;
        }
        private IList<BankTransaction> GetAccountTransactions(BankAccount account,  DateTime startTime, DateTime endTime)
        {
            var transactions = _api.GetTransactions(account, startTime, endTime);
            var result = new List<BankTransaction>();
            foreach (var transaction in transactions)
            {
                if (transaction.TransactionType.Equals("FUTURE")) { continue;}

                var eventDate = new DateTime((int) (transaction.EventDate / 10000), (int) (transaction.EventDate / 100 % 100),
                    (int) (transaction.EventDate % 100)).AddMinutes((int) (transaction.ExpandedEventDate % 100));
                result.Add(new BankTransaction
                {
                    Id = (transaction.ReferenceNumber + Math.Round(transaction.EventAmount) + Math.Round(transaction.CurrentBalance)).ToString(CultureInfo.InvariantCulture),
                    PurchaseDate = eventDate,
                    PaymentDate = eventDate,
                    Description = transaction.ActivityDescription,
                    ProviderName = ProviderName,
                    CurrentBalance = transaction.CurrentBalance,
                    Amount = transaction.EventAmount,
                    IsFee = transaction.ActivityTypeCode == (int)HapoalimActivityType.Fee,
                    Type = transaction.EventActivityTypeCode == 1 ? TransactionType.Income : TransactionType.Expense,
                    SupplierId = transaction.ReferenceNumber.ToString(),
                });
            }

            return result;
        }

        private IList<Mortgage> GetAccountMortgages(BankAccount account)
        {
            var mortgages = _api.GetMortgages(account);
            var result = new List<Mortgage>();
            if (mortgages.Data == null)
            {
                return result;
            }

            foreach (var mortgage in mortgages.Data)
            {
                if (!(account.AccountNumber == mortgage.AccountNumber &&
                      account.BranchNumber == mortgage.BranchNumber))
                {
                    continue;
                }

                var asset = _api.GetAssetForMortgage(account, mortgage.MortgageLoanSerialId);

                foreach (var subLoan in mortgage.SubLoanData)
                {
                    var startDate = (subLoan.ExecutingDate == 0) ? DateTime.MinValue : new DateTime((int)(subLoan.ExecutingDate / 10000), (int)(subLoan.ExecutingDate / 100 % 100),
                        (int)(subLoan.ExecutingDate % 100)).AddMinutes((int)(subLoan.ExecutingDate % 100));

                    var endDate = (subLoan.CalculatedEndDate == 0) ? DateTime.MinValue : new DateTime((int)(subLoan.CalculatedEndDate / 10000), (int)(subLoan.CalculatedEndDate / 100 % 100),
                        (int)(subLoan.CalculatedEndDate % 100)).AddMinutes((int)(subLoan.CalculatedEndDate % 100));

                    var ned = (subLoan.NextExitDate == 0) ? DateTime.MinValue : new DateTime((int)(subLoan.NextExitDate / 10000), (int)(subLoan.NextExitDate / 100 % 100),
                        (int)(subLoan.NextExitDate % 100)).AddMinutes((int)(subLoan.NextExitDate % 100));

                    var newMortgage = new Mortgage
                    {
                        LoanId = $"{mortgage.MortgageLoanSerialId}/{subLoan.SubLoansSerialId}",
                        StartDate = startDate,
                        EndDate = endDate,
                        DeptAmount = subLoan.PrincipalBalanceAmount,
                        OriginalAmount = subLoan.SubLoansPrincipalAmount,

                        InsuranceCompany = mortgage.LifeInsuranceCompanyName,
                        InterestType = mortgage.InterestTypeDescription,
                        LinkageType = mortgage.LinkageTypeDescription,

                        InterestRate = subLoan.ValidityInterestRate,
                        NextExitDate = ned,

                        Asset = new MortgageAsset
                        {
                            BuildingNumber = asset.BuildingNumber,
                            StreetName = asset.StreetName,
                            CityName = asset.CityName,
                            PartyFirstName = asset.PartyInMortgage.FirstOrDefault()?.PartyFirstName,
                            PartyLastName = asset.PartyInMortgage.FirstOrDefault()?.PartyLastName,
                        },
                    };

                    result.Add(newMortgage);
                }
            }

            return result;
        }

        private IList<Loan> GetAccountLoans(BankAccount account)
        {
            var loans = _api.GetLoans(account);
            var result = new List<Loan>();
            if (loans.Data == null)
            {
                return result;
            }

            foreach (var loan in loans.Data)
            {
                if (!(account.AccountNumber == loans.AccountNumber &&
                      account.BranchNumber == loans.BranchNumber))
                {
                    continue;
                }

                var details = _api.GetDetailsForLoan(account, loan);
                var startDate = (details.ValueDate == 0) ? DateTime.MinValue : new DateTime((int)(details.ValueDate / 10000), (int)(details.ValueDate / 100 % 100),
                    (int)(details.ValueDate % 100)).AddMinutes((int)(details.ValueDate % 100));
                var endDate = (details.LoanEndDate == 0) ? DateTime.MinValue : new DateTime((int)(details.LoanEndDate / 10000), (int)(details.LoanEndDate / 100 % 100),
                    (int)(details.LoanEndDate % 100)).AddMinutes((int)(details.LoanEndDate % 100));
                var nextPaymentDate = (loan.NextPaymentDate == 0) ? DateTime.MinValue : new DateTime((int)(loan.NextPaymentDate / 10000), (int)(loan.NextPaymentDate / 100 % 100),
                    (int)(loan.NextPaymentDate % 100)).AddMinutes((int)(loan.NextPaymentDate % 100));

                result.Add(new Loan
                {
                    LoanId = loan.CreditSerialNumber.ToString(),
                    StartDate = startDate,
                    EndDate = endDate,

                    OriginalAmount = loan.OriginalLoanPrincipalAmount,
                    DeptAmount = details.LoanBalanceAmount,
                    InterestRate = details.CurrentInterestPercent,
                    
                    NumberOfInterestPayments = details.OriginalPrincipalPaymentsNumber,
                    NumberOfPrincipalPayments = details.OriginalInterestPaymentsNumber,
                    NextInterestPayment = details.InterestNextPaymentNumber,
                    NextPrincipalPayment = details.PrincipalNextPaymentNumber,

                    NextPrepayment = loan.NextPaymentAmount,
                    NextPaymentDate = nextPaymentDate,
                });
            }

            return result;
        }
        #endregion

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
