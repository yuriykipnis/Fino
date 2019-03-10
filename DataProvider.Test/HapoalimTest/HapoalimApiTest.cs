using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataProvider.Providers.Exceptions;
using DataProvider.Providers.Models;
using DataProvider.Test.Controllers;
using Xunit;

namespace DataProvider.Test.HapoalimTest
{
    public class HapoalimApiTest : TestBase
    {
        [Fact]
        public void TestCreateApi_Fail_NoCredentials()
        {
            var exception = Assert.Throws<ArgumentException>(() => new Providers.Banks.Hapoalim.HapoalimApi(null));
            Assert.NotNull(exception);
        }

        [Fact]
        public void TestCreateApi_Fail_EmptyCredentials()
        {
            var credentials = new Dictionary<String, String>();
            var exception = Assert.Throws<ArgumentException>(() => new Providers.Banks.Hapoalim.HapoalimApi(credentials));
            Assert.NotNull(exception);
        }

        [Fact]
        public void TestCreateApi_Fail_MissingCredentials()
        {
            var credentials = new Dictionary<String, String> { { "", "" } };
            var exception = Assert.Throws<ArgumentException>(() => new Providers.Banks.Hapoalim.HapoalimApi(credentials));
            Assert.NotNull(exception);
        }

        [Fact]
        public void TestCreateApi_Fail_IncorrectCredentials()
        {
            var credentials = new Dictionary<String, String> { { "username", "abcdef" }, { "password", "123456" } };
            var exception = Assert.Throws<LoginException>(() => new Providers.Banks.Hapoalim.HapoalimApi(credentials));
            Assert.NotNull(exception);

            var isErrorCodeCorrect = exception.Error.Contains("Login Failure", StringComparison.InvariantCulture);
            Assert.True(isErrorCodeCorrect);
        }

        [Fact]
        public void TestCreateApi_Success()
        {
            var credentials = new Dictionary<String, String> { { "username", "vm61537" }, { "password", "w2e3r4t5" } };
            try
            {
                var api = new Providers.Banks.Hapoalim.HapoalimApi(credentials);
                api.Dispose();
            }
            catch (Exception ex)
            {
                Assert.Null(ex);
            }
        }

        [Fact]
        public void TestGetAccount_Success()
        {
            InitializeMapper();

            var credentials = new Dictionary<String, String> { { "username", "vm61537" }, { "password", "w2e3r4t5" } };
            var api = new Providers.Banks.Hapoalim.HapoalimApi(credentials);
            var accounts = api.GetAccounts().ToList();
            api.Dispose();

            Assert.NotNull(accounts);
            Assert.NotEmpty(accounts);

            foreach (var account in accounts)
            {
                Assert.NotEqual(0, account.BankNumber);
                Assert.NotEqual(0, account.ExtendedBankNumber);
                Assert.NotEqual(0, account.BranchNumber);
                Assert.NotEmpty(account.AccountNumber);
                Assert.NotEmpty(account.ProductLabel);
            }
        }

        [Fact]
        public void TestGetBalance_Success()
        {
            InitializeMapper();

            var credentials = new Dictionary<String, String> { { "username", "vm61537" }, { "password", "w2e3r4t5" } };
            var api = new Providers.Banks.Hapoalim.HapoalimApi(credentials);
            var accounts = api.GetAccounts().ToList();
            var hapoalimAccount = accounts.First(a => a.AccountClosingReasonCode == 0);
            var account = AutoMapper.Mapper.Map<BankAccount>(hapoalimAccount);
            account.Id = BankAccount.GenerateAccountId(account.BankNumber, account.BranchNumber, account.AccountNumber);

            var balance = api.GetBalance(account);
            api.Dispose();

            Assert.NotEqual(0, balance);
        }

        [Fact]
        public void TestGetTransactions_Success()
        {
            InitializeMapper();

            var credentials = new Dictionary<String, String> { { "username", "vm61537" }, { "password", "w2e3r4t5" } };
            var api = new Providers.Banks.Hapoalim.HapoalimApi(credentials);
            var accounts = api.GetAccounts().ToList();
            var hapoalimAccount = accounts.First(a => a.AccountClosingReasonCode == 0);
            var account = AutoMapper.Mapper.Map<BankAccount>(hapoalimAccount);
            account.Id = BankAccount.GenerateAccountId(account.BankNumber, account.BranchNumber, account.AccountNumber);

            var transactions = api.GetTransactions(account, DateTime.Now.AddMonths(-2), DateTime.Now.AddMonths(1));
            api.Dispose();

            Assert.NotNull(transactions);
            foreach (var transaction in transactions)
            {
                Assert.NotEqual(0, transaction.EventDate);
                Assert.NotEqual(0, transaction.ActivityTypeCode);
                Assert.NotEmpty(transaction.ActivityDescription);
                Assert.NotEqual(0, transaction.ReferenceNumber);
                Assert.NotEqual(0, transaction.ValueDate);
                Assert.NotEqual(0, transaction.EventAmount);
                Assert.NotEqual(0, transaction.EventActivityTypeCode);
                Assert.NotEqual(0, transaction.CurrentBalance);
                Assert.NotEmpty(transaction.TransactionType);
            }
        }

        [Fact]
        public void TestGetMortgages_Success()
        {
            InitializeMapper();

            var credentials = new Dictionary<String, String> { { "username", "vm61537" }, { "password", "w2e3r4t5" } };
            var api = new Providers.Banks.Hapoalim.HapoalimApi(credentials);
            var accounts = api.GetAccounts().ToList();
            var hapoalimAccount = accounts.First(a => a.AccountClosingReasonCode == 0);
            var account = AutoMapper.Mapper.Map<BankAccount>(hapoalimAccount);
            account.Id = BankAccount.GenerateAccountId(account.BankNumber, account.BranchNumber, account.AccountNumber);

            var mortgages = api.GetMortgages(account);
            api.Dispose();

            Assert.NotNull(mortgages);
            Assert.NotEmpty(mortgages.PartyId);

            if (mortgages.Data != null)
            {
                Assert.NotEqual(0, mortgages.PaymentAmount);
                Assert.NotEqual(0, mortgages.ValidityDate);

                foreach (var mortgage in mortgages.Data)
                {
                    Assert.NotEmpty(mortgage.AccountNumber);
                    Assert.NotEqual(0, mortgage.BankNumber);
                    Assert.NotEqual(0, mortgage.BranchNumber);
                    Assert.NotEqual(0, mortgage.CalculatedEndDate);
                    Assert.NotEqual(0, mortgage.ExecutingDate);
                    Assert.NotEmpty(mortgage.InterestTypeDescription);
                    Assert.NotEmpty(mortgage.LinkageTypeDescription);
                    Assert.NotEmpty(mortgage.MortgageLoanSerialId);
                    Assert.NotEqual(0, mortgage.PaymentAmount);
                    Assert.NotEqual(0, mortgage.RevaluedBalance);

                    Assert.NotNull(mortgage.SubLoanData);
                    foreach (var subLoan in mortgage.SubLoanData)
                    {
                        Assert.NotEqual(0, subLoan.AmountAndLinkageOfPrincipal);
                        Assert.NotEqual(0, subLoan.CalculatedEndDate);
                        Assert.NotEqual(0, subLoan.EndDate);
                        Assert.NotEqual(0, subLoan.ExecutingDate);
                        Assert.NotEqual(0, subLoan.InterestAmount);
                        Assert.NotEqual(0, subLoan.InterestAndLinkageTotalAmount);
                        Assert.NotEqual(0, subLoan.PrincipalBalanceAmount);
                        Assert.NotEqual(0, subLoan.StartDate);
                        Assert.NotEqual(0, subLoan.SubLoansPrincipalAmount);
                        Assert.NotEqual(0, subLoan.SubLoansSerialId);
                        Assert.NotEqual(0, subLoan.ValidityInterestRate);
                    }
                }
            }
        }

        [Fact]
        public void TestGetLoans_Success()
        {
            InitializeMapper();

            var credentials = new Dictionary<String, String> { { "username", "vm61537" }, { "password", "w2e3r4t5" } };
            var api = new Providers.Banks.Hapoalim.HapoalimApi(credentials);
            var accounts = api.GetAccounts().ToList();
            var hapoalimAccount = accounts.First(a => a.AccountClosingReasonCode == 0);
            var account = AutoMapper.Mapper.Map<BankAccount>(hapoalimAccount);
            account.Id = BankAccount.GenerateAccountId(account.BankNumber, account.BranchNumber, account.AccountNumber);

            var loans = api.GetLoans(account);
            api.Dispose();

            Assert.NotNull(loans);
            Assert.NotEqual(0, loans.BranchNumber);
            Assert.NotEmpty(loans.AccountNumber);
            Assert.NotEmpty(loans.ValidityDate);

            if (loans.Data != null)
            {
                foreach (var loan in loans.Data)
                {
                    Assert.NotEmpty(loan.CreditTypeDescription);
                    Assert.NotEmpty(loan.ExecutingPartyId);
                    Assert.NotEmpty(loan.FormattedNextPaymentDate);
                    Assert.NotEmpty(loan.PartyCatenatedLoanId);
                    Assert.NotEqual(0, loan.CreditCurrencyCode);
                    Assert.NotEqual(0, loan.CreditSerialNumber);
                    Assert.NotEqual(0, loan.OriginalLoanPrincipalAmount);
                    Assert.NotEqual(0, loan.NextPaymentAmount);
                    Assert.NotEqual(0, loan.NextPaymentDate);
                    Assert.NotEqual(0, loan.DebtAmount);
                    Assert.NotEqual(0, loan.LoanEndDate);
                }
            }
        }

        [Fact]
        public void TestGetAssetForMortgage_Success()
        {
            InitializeMapper();

            var credentials = new Dictionary<String, String> { { "username", "vm61537" }, { "password", "w2e3r4t5" } };
            var api = new Providers.Banks.Hapoalim.HapoalimApi(credentials);
            var accounts = api.GetAccounts().ToList();
            var hapoalimAccount = accounts.First(a => a.AccountClosingReasonCode == 0);
            var account = AutoMapper.Mapper.Map<BankAccount>(hapoalimAccount);
            account.Id = BankAccount.GenerateAccountId(account.BankNumber, account.BranchNumber, account.AccountNumber);

            var mortgages = api.GetMortgages(account).Data;
            var mortgage = mortgages.First();
            var asset = api.GetAssetForMortgage(account, mortgage.MortgageLoanSerialId);
            api.Dispose();

            Assert.NotNull(asset);

            Assert.NotEmpty(asset.CityName);
            Assert.NotEmpty(asset.StreetName);
            Assert.NotEmpty(asset.BuildingNumber);

            Assert.NotNull(asset.PartyInMortgage);
            Assert.NotEmpty(asset.PartyInMortgage);

            foreach (var party in asset.PartyInMortgage)
            {
                Assert.NotEmpty(party.CityName);
                Assert.NotEmpty(party.StreetName);
                Assert.NotEmpty(party.BuildingNumber);
                Assert.NotEmpty(party.PartyFirstName);
                Assert.NotEmpty(party.PartyLastName);
            }
        }

        [Fact]
        public void TestGetDetailsForLoan_Success()
        {
            InitializeMapper();

            var credentials = new Dictionary<String, String> { { "username", "vm61537" }, { "password", "w2e3r4t5" } };
            var api = new Providers.Banks.Hapoalim.HapoalimApi(credentials);
            var accounts = api.GetAccounts().ToList();
            var hapoalimAccount = accounts.First(a => a.AccountClosingReasonCode == 0);
            var account = AutoMapper.Mapper.Map<BankAccount>(hapoalimAccount);
            account.Id = BankAccount.GenerateAccountId(account.BankNumber, account.BranchNumber, account.AccountNumber);

            var loans = api.GetLoans(account).Data;
            var loan = loans.First();
            var details = api.GetDetailsForLoan(account, loan);
            api.Dispose();

            Assert.NotNull(details);

            Assert.NotEqual(0, details.ActualPrincipalBalance);
            Assert.NotEqual(0, details.CreditSerialNumber);
            Assert.NotEqual(0, details.CurrentInterestPercent);
            Assert.NotEmpty(details.FormattedLoanEndDate);
            Assert.NotEmpty(details.FormattedValueDate);
            Assert.NotEqual(0, details.InterestAmount);
            Assert.NotEqual(0, details.LoanEndDate);
            Assert.NotEqual(0, details.LoanBalanceAmount);
        }
    }
}
