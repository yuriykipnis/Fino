using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataProvider.Providers.Exceptions;
using DataProvider.Providers.Mapping;
using DataProvider.Test.Controllers;
using Xunit;

namespace DataProvider.Test.AmexTest
{
    public class CalApiTest : TestBase
    {
        [Fact]
        public void TestCreateApi_Fail_NoCredentials()
        {
            var exception = Assert.Throws<ArgumentException>(() => new Providers.Cards.Amex.AmexApi(null));
            Assert.NotNull(exception);
        }

        [Fact]
        public void TestCreateApi_Fail_EmptyCredentials()
        {
            var credentials = new Dictionary<String, String>();
            var exception = Assert.Throws<ArgumentException>(() => new Providers.Cards.Amex.AmexApi(credentials));
            Assert.NotNull(exception);
        }

        [Fact]
        public void TestCreateApi_Fail_MissingCredentials()
        {
            var credentials = new Dictionary<String, String> { { "", "" } };
            var exception = Assert.Throws<ArgumentException>(() => new Providers.Cards.Amex.AmexApi(credentials));
            Assert.NotNull(exception);
        }

        [Fact]
        public void TestCreateApi_Fail_IncorrectCredentials()
        {
            var credentials = new Dictionary<String, String> { { "ID", "abcdef" }, { "Last 6 digits", "123456" }, { "Password", "123456" } };
            var exception = Assert.Throws<LoginException>(() => new Providers.Cards.Amex.AmexApi(credentials));
            Assert.NotNull(exception);

            var isErrorCodeCorrect = exception.Error.Contains("Failed to login to Amex", StringComparison.InvariantCulture);
            Assert.True(isErrorCodeCorrect);
        }

        [Fact]
        public void TestCreateApi_Success()
        {
            var credentials = new Dictionary<String, String> { { "ID", "311913289" }, { "Last 6 digits", "742989" }, { "Password", "w2e3r4t5" } };
            try
            {
                var amexApi = new Providers.Cards.Amex.AmexApi(credentials);
                amexApi.Dispose();
            }
            catch (Exception ex)
            {
                Assert.Null(ex);
            }
        }
        
        [Fact]
        public void TestDisposeApi_Success()
        {
            var credentials = new Dictionary<String, String> { { "ID", "311913289" }, { "Last 6 digits", "742989" }, { "Password", "w2e3r4t5" } };
            var amexApi = new Providers.Cards.Amex.AmexApi(credentials);

            try
            {
                amexApi.Dispose();
            }
            catch (Exception ex)
            {
                Assert.Null(ex);
            }
        }

        [Fact]
        public void TestGetCards_Success()
        {
            var credentials = new Dictionary<String, String> { { "ID", "311913289" }, { "Last 6 digits", "742989" }, { "Password", "w2e3r4t5" } };
            var amexApi = new Providers.Cards.Amex.AmexApi(credentials);
            var cards = amexApi.GetCards();
            amexApi.Dispose();

            Assert.True(cards.Any());
        }

        [Fact]
        public void TestGetValidCards_Success()
        {
            var credentials = new Dictionary<String, String> { { "ID", "311913289" }, { "Last 6 digits", "742989" }, { "Password", "w2e3r4t5" } };
            var amexApi = new Providers.Cards.Amex.AmexApi(credentials);
            var cards = amexApi.GetCards();
            amexApi.Dispose();

            Assert.True(cards.Count() == 2);
        }

        [Fact]
        public void TestGetBankDebit_Success()
        {
            var credentials = new Dictionary<String, String> { { "ID", "311913289" }, { "Last 6 digits", "742989" }, { "Password", "w2e3r4t5" } };
            var amexApi = new Providers.Cards.Amex.AmexApi(credentials);
            
            var debits = amexApi.GetBankDebit("129249", "2989", DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month);
            amexApi.Dispose();

            Assert.True(debits.All(c => c.CardNumber == "2989"));
        }

        [Fact]
        public void TestGetBankDebit_Fail()
        {
            var credentials = new Dictionary<String, String> { { "ID", "311913289" }, { "Last 6 digits", "742989" }, { "Password", "w2e3r4t5" } };
            var amexApi = new Providers.Cards.Amex.AmexApi(credentials);
            var exception = Assert.Throws<Exception>(() => amexApi.GetBankDebit("123449", "2489", DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month));
            amexApi.Dispose();

            Assert.NotNull(exception);
        }

        [Fact]
        public void TestGetTransactions_Success()
        {
            var credentials = new Dictionary<String, String> { { "ID", "311913289" }, { "Last 6 digits", "742989" }, { "Password", "w2e3r4t5" } };
            var amexApi = new Providers.Cards.Amex.AmexApi(credentials);
            var transactions = amexApi.GetTransactions(0, DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month);
            amexApi.Dispose();

            Assert.True(transactions.Any());
        }

        [Fact]
        public void TestGetTransactions_Failed()
        {
            var credentials = new Dictionary<String, String> { { "ID", "311913289" }, { "Last 6 digits", "742989" }, { "Password", "w2e3r4t5" } };
            var amexApi = new Providers.Cards.Amex.AmexApi(credentials);
            var exception = Assert.Throws<Exception>(() => amexApi.GetTransactions(10, DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month));
            amexApi.Dispose();

            Assert.NotNull(exception);
        }

        [Fact]
        public void TestGetTransactionDetails_Success()
        {
            var credentials = new Dictionary<String, String> { { "ID", "311913289" }, { "Last 6 digits", "742989" }, { "Password", "w2e3r4t5" } };
            var amexApi = new Providers.Cards.Amex.AmexApi(credentials);
            var transactions = amexApi.GetTransactions(0, DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month);

            foreach (var trs in transactions)
            {
                var isInbound = !string.IsNullOrEmpty(trs.DealsInbound) && !trs.DealsInbound.Equals("no", StringComparison.CurrentCultureIgnoreCase);
                var voucher = string.IsNullOrEmpty(trs.DealsInbound)
                    ? trs.VoucherNumberRatzOutbound
                    : trs.VoucherNumberRatz;

                if (Convert.ToInt64(voucher) == 0) continue;

                var deatils = amexApi.GetTransactionDetails(0, "012019", isInbound, voucher);
                
                Assert.NotEmpty(deatils.Sector);
                if (isInbound)
                {
                    Assert.NotEmpty(deatils.Address);
                }
            }

            amexApi.Dispose();
        }
    }
}
