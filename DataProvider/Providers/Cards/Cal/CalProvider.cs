using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataProvider.Providers.Cards.Cal.Dto;
using DataProvider.Providers.Interfaces;
using DataProvider.Providers.Models;

namespace DataProvider.Providers.Cards.Cal
{
    public class CalProvider : ICreditCardProvider
    {
        private const string ProviderName = "Visa Cal";
        private readonly ICalApi _api;

        public CalProvider(ICalApi api)
        {
            _api = api;
        }

        public IEnumerable<CreditCard> GetCards()
        {
            var result = new List<CreditCard>();
            var accountsData = _api.GetCards();
            foreach (var bankAccount in accountsData.BankAccounts)
            {
                var newCards = GenerateCreditCards(bankAccount, accountsData.CustomerInfo);
                result.AddRange(newCards);
            }

            return result;
        }
        
        public IEnumerable<CreditCard> GetCardsWithTransactions(List<CreditCardDescriptor> creditCardDescriptor, 
                                                      DateTime startDate, DateTime endDate, bool includeDeatils = false)
        {
            var result = new List<CreditCard>();
            var accountsData = _api.GetCards();

            foreach (var bankAccount in accountsData.BankAccounts)
            {
                var cards = bankAccount.Cards.Where(c => {
                    return creditCardDescriptor.Any(d => d.CardId == c.Id);
                });
                
                foreach (var card in cards)
                {
                    var newCard = GenerateCreditCard(bankAccount, card, accountsData.CustomerInfo);
                    newCard.Debits = GetDebits(bankAccount, card, startDate, endDate, includeDeatils);
                    result.Add(newCard);
                }
            }

            return result;
        }

        private IList<CreditCardDebitPeriod> GetDebits(CalAccountResponse bankAccount, CalCardResponse card, DateTime startDate, DateTime endDate, bool includeDeatils)
        {
            var result = new List<CreditCardDebitPeriod>();
            var debits = _api.GetBankDebits(bankAccount.AccountId, card.Id, startDate, endDate.AddMonths(1)).ToList();

            var sd = debits.First().Date;
            var ed = debits.Last().Date;
            var transactions = _api.GetTransactions(card.Id, sd, ed).ToList();

            foreach (var debit in debits)
            {
                var newDebit = AutoMapper.Mapper.Map<CreditCardDebitPeriod>(debit);
                var transactionsForDebit = transactions.Where(t =>
                {
                    var transactionDebitDate = AutoMapper.Mapper.Map<DateTime>(t.DebitDate);
                    return transactionDebitDate.Equals(debit.Date);
                });

                var trss = includeDeatils
                    ? GetEnrichedTransactions(transactionsForDebit)
                    : GetTransactions(transactionsForDebit);

                (newDebit.Transactions as List<CreditCardTransaction>)?.AddRange(trss);

                result.Add(newDebit);
            }

            return result;
        }

        private IList<CreditCardTransaction> GetTransactions(IEnumerable<CalTransactionResponse> transactionsForDebit)
        {
            return AutoMapper.Mapper.Map<IList<CreditCardTransaction>>(transactionsForDebit);
        }

        private IList<CreditCardTransaction> GetEnrichedTransactions(IEnumerable<CalTransactionResponse> transactionsForDebit)
        {
            var tasks = CreateEnrichmentTasks(transactionsForDebit);

            foreach (var task in tasks)
            {
                task.Start();
            }

            Task.WaitAll(tasks.ToArray());

            var result = GenerateEnrichedTransactions(tasks);
            return result;
        }

        private static IList<CreditCardTransaction> GenerateEnrichedTransactions(IEnumerable<Task<CalTransactionDetailsResponse>> tasks)
        {
            var result = new List<CreditCardTransaction>();

            foreach (var task in tasks)
            {
                var newTransaction = AutoMapper.Mapper.Map<CreditCardTransaction>(task.AsyncState);
                var details = task.Result;

                newTransaction.DealSector = details.Data.MerchantDetails.SectorName ?? string.Empty;
                newTransaction.SupplierAddress = details.Data.MerchantDetails.Address ?? string.Empty;
                result.Add(newTransaction);
            }

            return result;
        }

        private IList<Task<CalTransactionDetailsResponse>> CreateEnrichmentTasks(IEnumerable<CalTransactionResponse> transactionsForDebit)
        {
            var result = new List<Task<CalTransactionDetailsResponse>>();
            foreach (var transaction in transactionsForDebit)
            {
                var task = new Task<CalTransactionDetailsResponse>((state) =>
                {
                    var trs = (CalTransactionResponse) state;
                    return _api.GetTransactionDetails(trs.Id, trs.Numerator);
                }, transaction);

                result.Add(task);
            }

            return result;
        }

        private static IList<CreditCard> GenerateCreditCards(CalAccountResponse account, CalCustomerInfoResponse customer)
        {
            var result = new List<CreditCard>();
            foreach (var card in account.Cards)
            {
                var creditCard = GenerateCreditCard(account, card, customer);
                result.Add(creditCard);
            };

            return result;
        }

        private static CreditCard GenerateCreditCard(CalAccountResponse account, CalCardResponse card, CalCustomerInfoResponse customer)
        {
            return new CreditCard
            {
                Id = card.Id,
                CardName = $"{card.NumTypeDescription} {card.NumType}",
                LastDigits = card.LastFourDigits,
                OwnerFirstName = card.OwnerFirstName,
                OwnerLastName = card.OwnerLastName,
                TypeDescription = card.NumTypeDescription,
                HolderId = customer.Id,
                CardAccount = AutoMapper.Mapper.Map<CreditCardBankAccount>(account),
                Debits = new List<CreditCardDebitPeriod>()
            };
        }

        public void Dispose()
        {
            _api.Dispose();
        }
    }
}
