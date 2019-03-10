using System;
using System.Collections.Generic;
using System.Linq;
using DataProvider.Providers.Cards;
using DataProvider.Providers.Cards.Cal;
using DataProvider.Providers.Cards.Cal.Dto;
using DataProvider.Providers.Mapping;
using DataProvider.Providers.Models;
using DataProvider.Test.Controllers;
using Xunit;

namespace DataProvider.Test.CalTest
{
    public class CalProviderTest : TestBase
    {
        [Fact]
        public void TestGetCards_Success()
        {
            InitializeMapper();
            var credentials = new Dictionary<String, String> { { "username", "YURIYK81" }, { "password", "2w3e4r5t" } };
            var api = new Providers.Cards.Cal.CalApi(credentials);
            var provider = new CalProvider(api);
            var cards = provider.GetCards();

            Assert.NotEmpty(cards);
            foreach (var card in cards)
            {
                Assert.NotNull(card.Debits);
                Assert.NotEmpty(card.Id);
                Assert.NotEmpty(card.HolderId);
                Assert.NotEmpty(card.TypeDescription);
                Assert.NotEmpty(card.CardName);
                Assert.NotEmpty(card.LastDigits);
                Assert.NotEmpty(card.OwnerFirstName);
                Assert.NotEmpty(card.OwnerLastName);

                Assert.NotNull(card.CardAccount);
                Assert.NotEmpty(card.CardAccount.AccountNumber);
                Assert.NotEmpty(card.CardAccount.Id);
                Assert.NotEmpty(card.CardAccount.BankName);
                Assert.True(card.CardAccount.BankBranchNumber > 0);
                Assert.True(card.CardAccount.BankCode > 0);
            }
        }

        [Fact]
        public void TestGetCardsWithTransactions_Success()
        {
            InitializeMapper();
            var credentials = new Dictionary<String, String> { { "username", "YURIYK81" }, { "password", "2w3e4r5t" } };
            var api = new CalApi(credentials);
            var provider = new CalProvider(api);
            var descriptors = new List<CreditCardDescriptor>
            {
                new CreditCardDescriptor
                {
                    CardId = "01981802"
                }
            };
            
            var cards = provider.GetCardsWithTransactions(descriptors, DateTime.Now.AddMonths(-1), DateTime.Now);

            foreach (var card in cards)
            {
                Assert.NotEmpty(card.Id);
                Assert.NotEmpty(card.HolderId);
                Assert.NotEmpty(card.TypeDescription);
                Assert.NotEmpty(card.CardName);
                Assert.NotEmpty(card.LastDigits);
                Assert.NotEmpty(card.OwnerFirstName);
                Assert.NotEmpty(card.OwnerLastName);

                Assert.NotNull(card.CardAccount);
                Assert.NotEmpty(card.CardAccount.AccountNumber);
                Assert.NotEmpty(card.CardAccount.Id);
                Assert.NotEmpty(card.CardAccount.BankName);
                Assert.True(card.CardAccount.BankBranchNumber > 0);
                Assert.True(card.CardAccount.BankCode > 0);

                Assert.NotNull(card.Debits);
                Assert.NotEmpty(card.Debits);
                Assert.True(card.Debits.First().Date > DateTime.MinValue);
                Assert.NotEmpty(card.Debits.First().CardId);
                Assert.NotEmpty(card.Debits.First().CardLastDigits);
                Assert.True(card.Debits.First().Amount >= 0);

                foreach (var debit in card.Debits)
                {
                    Assert.NotEmpty(debit.CardId);
                    Assert.True(debit.Date > DateTime.MinValue);
                    Assert.NotEmpty(debit.CardLastDigits);

                    Decimal payment = 0;
                    foreach (var transaction in debit.Transactions)
                    {
                        payment = payment + transaction.PaymentAmount;
                    }
                    Assert.True(debit.Amount == payment);
                }
            }
        }

        [Fact]
        public void TestGetCardsWithTransactionsWithDetails_Success()
        {
            InitializeMapper();
            var credentials = new Dictionary<String, String> { { "username", "YURIYK81" }, { "password", "2w3e4r5t" } };
            var api = new Providers.Cards.Cal.CalApi(credentials);
            var provider = new CalProvider(api);
            var descriptors = new List<CreditCardDescriptor>
            {
                new CreditCardDescriptor
                {
                    CardId = "01981802"
                }
            };

            var cards = provider.GetCardsWithTransactions(descriptors, DateTime.Now.AddMonths(-1), DateTime.Now, true);

            foreach (var card in cards)
            {
                Assert.NotEmpty(card.Id);
                Assert.NotEmpty(card.HolderId);
                Assert.NotEmpty(card.TypeDescription);
                Assert.NotEmpty(card.CardName);
                Assert.NotEmpty(card.LastDigits);
                Assert.NotEmpty(card.OwnerFirstName);
                Assert.NotEmpty(card.OwnerLastName);

                Assert.NotNull(card.CardAccount);
                Assert.NotEmpty(card.CardAccount.AccountNumber);
                Assert.NotEmpty(card.CardAccount.Id);
                Assert.NotEmpty(card.CardAccount.BankName);
                Assert.True(card.CardAccount.BankBranchNumber > 0);
                Assert.True(card.CardAccount.BankCode > 0);

                Assert.NotNull(card.Debits);
                Assert.NotEmpty(card.Debits);
                Assert.True(card.Debits.First().Date > DateTime.MinValue);
                Assert.NotEmpty(card.Debits.First().CardId);
                Assert.NotEmpty(card.Debits.First().CardLastDigits);
                Assert.True(card.Debits.First().Amount >= 0);

                foreach (var debit in card.Debits)
                {
                    Assert.NotEmpty(debit.CardId);
                    Assert.True(debit.Date > DateTime.MinValue);
                    Assert.NotEmpty(debit.CardLastDigits);

                    Decimal payment = 0;
                    foreach (var transaction in debit.Transactions)
                    {
                        payment = payment + transaction.PaymentAmount;
                    }
                    Assert.True(debit.Amount == payment);
                }
            }
        }
    }
}
