using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataProvider.Controllers;
using DataProvider.Providers;
using DataProvider.Providers.Models;
using DataProvider.Services;
using GoldMountainShared.Dto;
using GoldMountainShared.Dto.Bank;
using GoldMountainShared.Dto.Credit;
using GoldMountainShared.Dto.Provider;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Repositories;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace DataProvider.Test.Controllers
{
    public class CreditCardControllerTest : ControllerTestBase, IDisposable
    {
        private String _userId;
        private CreditCardController _controller;

        public CreditCardControllerTest()
        {
            InitializeController();
            InitializeUserRepository();
        }

        [Fact]
        public void GetTest_Success()
        {
            InitializeMapper();
            var cardIds = new List<String> { "01981802" };
            var cardsToTest = InitializeCreditCardRepository("123", cardIds, DateTime.Now);

            var getResult = _controller.Get().Result;
            var okGetResult = getResult as OkObjectResult;
            var cards = okGetResult?.Value as IEnumerable<CreditCardDto>;

            CompareCards(cards, cardsToTest);

            //test the repostiry and make sure that the data is in place and correct
        }

        [Fact]
        public void GetUpdatedTest_Success()
        {
            InitializeMapper();
            var cardIds = new List<String> { "01981802" };

            var providerId = InitializeProvidersRepository(cardIds);
            var cardsToTest = InitializeCreditCardRepository( providerId, cardIds, DateTime.Now.AddDays(-3));

            var getResult = _controller.UpdateCards(_userId).Result;
            var okGetResult = getResult as OkObjectResult;
            var cards = okGetResult?.Value as IEnumerable<CreditCardDto>;

            CompareCards(cards, cardsToTest);

            //test the repostiry and make sure that the data is in place and correct
        }


        [Fact]
        public void PostTest_Success()
        {
            InitializeMapper();

            var postResult = _controller.GetCards(new ProviderCreatingDto
            {
                UserId = _userId.ToString(),
                Name = "Visa Cal",
                Type = InstitutionType.Credit,
                Credentials = new Dictionary<String, String> { { "username", "YURIYK81" }, { "password", "2w3e4r5t" } },
                BankAccounts = new List<BankAccountDto>(),
                CreditCards = new List<CreditCardDto>()
            }).Result;

            var okPostResult = postResult as OkObjectResult;
            var cards = okPostResult?.Value as IEnumerable<CreditCardDto>;
            
            Assert.NotNull(cards);
            Assert.NotEmpty(cards);

            foreach (var card in cards)
            {

                Assert.NotEmpty(card.Id);
                Assert.NotEmpty(card.LastDigits);
                Assert.NotEmpty(card.CardName);
                Assert.NotEmpty(card.TypeDescription);
                Assert.NotEmpty(card.Id);

                foreach (var debit in card.Debits)
                {
                    var debitToTest = card.Debits.First(d => d.Date == debit.Date);

                    Assert.True(debit.Date < DateTime.Now);
                    Assert.NotNull(debit.Transactions);
                    Assert.NotEmpty(debit.Transactions);

                    foreach (var transaction in debit.Transactions)
                    {
                        var transactionToTest = debitToTest.Transactions.First(t => t.Id == transaction.Id);

                        Assert.Equal(transaction.Id, transactionToTest.Id);
                        Assert.Equal(transaction.PaymentAmount, transactionToTest.PaymentAmount);
                        Assert.Equal(transaction.PaymentCurrency, transactionToTest.PaymentCurrency);
                        Assert.Equal(transaction.PaymentDate, transactionToTest.PaymentDate);
                        Assert.Equal(transaction.DealAmount, transactionToTest.DealAmount);
                        Assert.Equal(transaction.DealCurrency, transactionToTest.DealCurrency);
                        Assert.Equal(transaction.DealDate, transactionToTest.DealDate);
                        Assert.Equal(transaction.Description, transactionToTest.Description);
                        Assert.Equal(transaction.Notes, transactionToTest.Notes);
                        Assert.Equal(transaction.DealSector, transactionToTest.DealSector);
                    }
                }
            }
        }

        private void InitializeController()
        {
            var bankAccountRepository = new BankAccountRepository(_options);
            var creditCardRepository = new CreditCardRepository(_options);
            var providerRepository = new ProviderRepository(_options);
            var providerFactory = new ProviderFactory();
            var accountService = new AccountService(providerFactory, bankAccountRepository, creditCardRepository);

            _controller = new CreditCardController(providerFactory, providerRepository, 
                                                   accountService, creditCardRepository);
        }

        private void InitializeUserRepository()
        {
            CleanUserRepository();

            var usersRepository = new UserRepository(_options);
            var result = usersRepository.AddUser(new UserDoc
            {
                Name = "Tester",
                Email = "tester@gmail.com",
                Accounts = new List<String>()
            });

            _userId = usersRepository.GetAllUsers().Result.FirstOrDefault()?.Id;
        }

        private String InitializeProvidersRepository(IEnumerable<String> cards)
        {
            CleanProvidersRepository();

            var providersRepository = new ProviderRepository(_options);

            return providersRepository.AddProvider(new ProviderDoc
            {
                UserId = _userId,
                Type = InstitutionType.Credit,
                Name = "Visa Cal",
                Credentials = new Dictionary<String, String>
                {
                    { "username", "YURIYK81" },
                    { "password", "2w3e4r5t" }
                },
                Accounts = cards
            }).Result;
        }

        private IList<CreditCard> InitializeCreditCardRepository(string providerId, IList<String> cardIds, DateTime lastUpdate)
        {
            InitializeMapper();

            var creditCardRepository = new CreditCardRepository(_options);
            var delResult = creditCardRepository.RemoveAllCards();

            var cards = new List<CreditCard>();
            cards.Add(new CreditCard
            {
                Id = cardIds[0],
                LastDigits = "2989",
                CardName = "2989 - אמקס BUSINESS זהב",
                TypeDescription = "טכניון תארים גבוהים",
                OwnerFirstName = "יורי",
                OwnerLastName = "קיפניס",
                HolderId = "311913289",
                CardAccount = new CreditCardBankAccount
                {
                    Id = "12-476-129249",
                    AccountNumber = "129249",
                    BankName = "בנק הפועלים",
                    BankBranchNumber = 476,
                    BankCode = 12
                },
                Debits = new List<CreditCardDebitPeriod>
                {
                    new CreditCardDebitPeriod
                    {
                        CardId = "12-476-129249-2989",
                        CardLastDigits = "2989",
                        Date = DateTime.Now.AddMonths(-1),
                        Amount = 1400,
                        Transactions = new List<CreditCardTransaction>
                        {
                            new CreditCardTransaction
                            {
                                Id = "123456",
                                PaymentCurrency = "שח",
                                PaymentDate = DateTime.Now.AddMonths(-1),
                                PaymentAmount = 1200,
                                DealCurrency = "שח",
                                DealAmount = 1200,
                                DealDate = DateTime.Now.AddMonths(-1),
                                Description = "פניקס חיים ובריאות",
                                Notes = "",
                                SupplierAddress = "תל אביב קרליבך 23",
                                DealSector = "ביטוח"
                            },
                            new CreditCardTransaction
                            {
                                Id = "4562341",
                                PaymentCurrency = "שח",
                                PaymentDate = DateTime.Now.AddMonths(-1),
                                PaymentAmount = 200,
                                DealCurrency = "שח",
                                DealAmount = 200,
                                DealDate = DateTime.Now.AddMonths(-1),
                                Description = "יקב טורא ארץ הצבי",
                                Notes = "",
                                SupplierAddress = "נופי נחמיה 456",
                                DealSector = "צרכנות"
                            },
                        }
                    },
                    new CreditCardDebitPeriod
                    {
                        CardId = "12-476-129249-2989",
                        CardLastDigits = "2989",
                        Date = DateTime.Now.AddMonths(-2),
                        Amount = 1000,
                        Transactions = new List<CreditCardTransaction>
                        {
                            new CreditCardTransaction
                            {
                                Id = "34523452",
                                PaymentCurrency = "שח",
                                PaymentDate = DateTime.Now.AddDays(-23),
                                PaymentAmount = 1000,
                                DealCurrency = "שח",
                                DealAmount = 1000,
                                DealDate = DateTime.Now.AddDays(-23),
                                Description = "משיכת מזומנים",
                                Notes = "",
                                SupplierAddress = "נתניה מרכז הששרון",
                                DealSector = "מזומן"
                            }
                        }
                    },
                }
            });

            var cardsDoc = AutoMapper.Mapper.Map<IEnumerable<CreditCardDoc>>(cards);
            foreach (var cardDoc in cardsDoc)
            {
                cardDoc.ProviderName = "Amex";
                cardDoc.ProviderId = providerId;
                cardDoc.UpdatedOn = lastUpdate;
            }


            var result = creditCardRepository.AddCards(cardsDoc);
            
            return cards;
        }

        private static void CompareCards(IEnumerable<CreditCardDto> cards, IList<CreditCard> cardsToTest)
        {
            Assert.NotNull(cards);
            Assert.NotEmpty(cards);

            foreach (var card in cards)
            {
                var testCard = cardsToTest.First(c => c.Id == card.Id);

                Assert.Equal(card.Id, testCard.Id);
                Assert.Equal(card.LastDigits, testCard.LastDigits);
                Assert.Equal(card.CardName, testCard.CardName);
                Assert.Equal(card.TypeDescription, testCard.TypeDescription);
                Assert.Equal(card.Id, testCard.Id);

                foreach (var debit in card.Debits)
                {
                    Assert.NotNull(debit.Transactions);
                    Assert.NotEmpty(debit.Transactions);

                    var debitToTest = testCard.Debits.FirstOrDefault(d => d.Date.Date == debit.Date.Date);
                    if (debitToTest == null) continue;

                    Assert.Equal(debit.Amount, debitToTest?.Amount);

                    foreach (var transaction in debit.Transactions)
                    {
                        var transactionToTest = debitToTest.Transactions.First(t => t.Id == transaction.Id);

                        Assert.Equal(transaction.Id, transactionToTest.Id);
                        Assert.Equal(transaction.PaymentAmount, transactionToTest.PaymentAmount);
                        Assert.Equal(transaction.PaymentCurrency, transactionToTest.PaymentCurrency);
                        Assert.Equal(transaction.PaymentDate.Date, transactionToTest.PaymentDate.Date);
                        Assert.Equal(transaction.DealAmount, transactionToTest.DealAmount);
                        Assert.Equal(transaction.DealCurrency, transactionToTest.DealCurrency);
                        Assert.Equal(transaction.DealDate.Date, transactionToTest.DealDate.Date);
                        Assert.Equal(transaction.Description, transactionToTest.Description);
                        Assert.Equal(transaction.Notes, transactionToTest.Notes);
                        Assert.Equal(transaction.DealSector, transactionToTest.DealSector);
                    }
                }
            }
        }

        private void CleanProvidersRepository()
        {
            var providersRepository = new ProviderRepository(_options);
            var result = providersRepository.RemoveAllProviders();
        }

        private void CleanUserRepository()
        {
            var usersRepository = new UserRepository(_options);
            var result = usersRepository.RemoveAllUsers();
        }

        private void CleanCardsRepository()
        {
            var creaditCardRepository = new CreditCardRepository(_options);
            var result = creaditCardRepository.RemoveAllCards();
        }

        public void Dispose()
        {
            CleanProvidersRepository();
            CleanUserRepository();
            CleanCardsRepository();
        }
    }
}
