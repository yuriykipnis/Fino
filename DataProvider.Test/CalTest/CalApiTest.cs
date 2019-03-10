using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using DataProvider.Providers.Exceptions;
using DataProvider.Providers.Mapping;
using DataProvider.Test.Controllers;
using Xunit;

namespace DataProvider.Test.CalTest
{
    public class CalApiTest : TestBase
    {
        
        [Fact]
        public void TestCreateApi_Fail_NoCredentials()
        {
            var exception = Assert.Throws<ArgumentException>(() => new Providers.Cards.Cal.CalApi(null));
            Assert.NotNull(exception);
        }

        [Fact]
        public void TestCreateApi_Fail_EmptyCredentials()
        {
            var  credentials = new Dictionary<String, String>();
            var exception = Assert.Throws<ArgumentException>(() => new Providers.Cards.Cal.CalApi(credentials));
            Assert.NotNull(exception);
        }
        
        [Fact]
        public void TestCreateApi_Fail_MissingCredentials()
        {
            var credentials = new Dictionary<String, String> { {"", ""} };
            var exception = Assert.Throws<ArgumentException>(() => new Providers.Cards.Cal.CalApi(credentials));
            Assert.NotNull(exception);
        }

        [Fact]
        public void TestCreateApi_Fail_IncorrectCredentials()
        {
            var credentials = new Dictionary<String, String> { { "username", "abcdef" }, { "password", "123456" } };
            var exception = Assert.Throws<LoginException>(() => new Providers.Cards.Cal.CalApi(credentials));
            Assert.NotNull(exception);

            var isErrorCodeCorrect = exception.Error.Contains("שם המשתמש או הסיסמה שהוזנו שגויים", StringComparison.InvariantCulture);
            Assert.True(isErrorCodeCorrect);
        }

        [Fact]
        public void TestCreateApi_Success()
        {
            var credentials = new Dictionary<String, String> { { "username", "YURIYK81" }, { "password", "2w3e4r5t" } };
            try
            {
                var calApi = new Providers.Cards.Cal.CalApi(credentials);
                calApi.Dispose();
            }
            catch (Exception ex)
            {
                Assert.Null(ex);
            }
        }

        [Fact]
        public void TestDisposeApi_Fail()
        {
            // How to test the negative flow?

            //var credentials = new Dictionary<string, string> { { "username", "YURIYK81" }, { "password", "2w3e4r5t" } };
            //var calApi = new Providers.Cards.Cal.CalApi(credentials);

            //var exception = Assert.Throws<Exception>(() => calApi.Dispose());
            //Assert.NotNull(exception);

            //var isErrorCodeCorrect = exception.Message.StartsWith("Exit in Cal api failed", StringComparison.InvariantCulture);
            //Assert.True(isErrorCodeCorrect);
        }

        [Fact]
        public void TestDisposeApi_Success()
        {
            var credentials = new Dictionary<string, string> { { "username", "YURIYK81" }, { "password", "2w3e4r5t" } };
            var calApi = new Providers.Cards.Cal.CalApi(credentials);
            
            try
            {
                calApi.Dispose();
            }
            catch (Exception ex)
            {
                Assert.Null(ex);
            }
        }

        [Fact]
        public void TestGetCards_Success()
        {
            var credentials = new Dictionary<string, string> { { "username", "YURIYK81" }, { "password", "2w3e4r5t" } };
            var calApi = new Providers.Cards.Cal.CalApi(credentials);
            var cards = calApi.GetCards();
            calApi.Dispose();

            Assert.True(cards.BankAccounts.First().Cards.Any());
        }

        [Fact]
        public void TestGetValidCards_Success()
        {
            var credentials = new Dictionary<string, string> { { "username", "YURIYK81" }, { "password", "2w3e4r5t" } };
            var calApi = new Providers.Cards.Cal.CalApi(credentials);
            var cards = calApi.GetCards();
            calApi.Dispose();

            Assert.True(cards.BankAccounts.First().Cards.Count() == 2);
        }

        [Fact]
        public void TestGetBankDebits_Success()
        {
            InitializeMapper();

            var credentials = new Dictionary<string, string> { { "username", "YURIYK81" }, { "password", "2w3e4r5t" } };
            var calApi = new Providers.Cards.Cal.CalApi(credentials);
            var debits = calApi.GetBankDebits("00527482012617", "01981802", DateTime.Now.AddMonths(-1), DateTime.Now);
            calApi.Dispose();

            Assert.True(debits.Any());
        }

        [Fact]
        public void TestGetBankDebits_Fail()
        {
            var credentials = new Dictionary<string, string> { { "username", "YURIYK81" }, { "password", "2w3e4r5t" } };
            var calApi = new Providers.Cards.Cal.CalApi(credentials);
            var exception = Assert.Throws<Exception>(() => 
                calApi.GetBankDebits("0052748202617", "01981802", DateTime.Now.AddMonths(-1), DateTime.Now));
            calApi.Dispose();

            Assert.NotNull(exception);
            Assert.StartsWith("Did not succeed to fetch debits for card 01981802", exception.Message, StringComparison.CurrentCultureIgnoreCase);
        }

        [Fact]
        public void TestGetTransactions_Success()
        {
            var credentials = new Dictionary<string, string> { { "username", "YURIYK81" }, { "password", "2w3e4r5t" } };
            var calApi = new Providers.Cards.Cal.CalApi(credentials);
            var transactions = calApi.GetTransactions("01981802", DateTime.Now.AddMonths(-1), DateTime.Now).ToList();
            calApi.Dispose();

            Assert.True(transactions.Any());
            foreach (var transaction in transactions)
            {
                Assert.NotEmpty(transaction.Id);
                Assert.NotEmpty(transaction.Currency);
                Assert.NotEmpty(transaction.CurrentPayment);
                Assert.NotEmpty(transaction.Date);
                Assert.NotEmpty(transaction.DebitDate);
                //Assert.NotEmpty(transaction.Notes);
                //Assert.NotEmpty(transaction.Comments);
            }
        }

        //[Fact]
        //public void TestGetTransactions_Failed()
        //{
        //    var credentials = new Dictionary<string, string> { { "username", "YURIYK81" }, { "password", "2w3e4r5t" } };
        //    var calApi = new Providers.Cards.Cal.CalApi(credentials);
        //    var exception = Assert.Throws<Exception>(() =>
        //        calApi.GetTransactions("0198180", new DateTime(2019, 1, 1), new DateTime(2019, 2, 1)));
        //    calApi.Dispose();

        //    Assert.NotNull(exception);
        //    Assert.StartsWith("Did not succeed to fetch transaction in card 0198180", exception.Message, StringComparison.CurrentCultureIgnoreCase);
        //}

        [Fact]
        public void TestGetTransactionDetails_Success()
        {
            var credentials = new Dictionary<string, string> { { "username", "YURIYK81" }, { "password", "2w3e4r5t" } };
            var calApi = new Providers.Cards.Cal.CalApi(credentials);
            var transactions = calApi.GetTransactions("01981802", DateTime.Now.AddMonths(-1), DateTime.Now).ToList();

            foreach (var transaction in transactions)
            {
                var transactionId = transaction.Id;
                var numerator = transaction.Numerator;
                var details = calApi.GetTransactionDetails(transactionId, numerator);

                Assert.NotNull(details);
                Assert.NotNull(details.Data);
                Assert.NotNull(details.Data.MerchantDetails);
                //Assert.NotNull(details.Data.MerchantDetails.Address);
                //Assert.NotNull(details.Data.MerchantDetails.SectorName);
            }
            
            calApi.Dispose();
        }

        [Fact]
        public void TestGetTransactionDetails_Failed()
        {
            var credentials = new Dictionary<string, string> { { "username", "YURIYK81" }, { "password", "2w3e4r5t" } };
            var calApi = new Providers.Cards.Cal.CalApi(credentials);
            var transactions = calApi.GetTransactions("01981802", DateTime.Now.AddMonths(-1), DateTime.Now).ToList();

            var transactionId = transactions.First().Id.Substring(1);
            var numerator = transactions.First().Numerator;

            var exception = Assert.Throws<Exception>(() => calApi.GetTransactionDetails(transactionId, numerator));
            calApi.Dispose();

            Assert.NotNull(exception);
        }
    }
}
