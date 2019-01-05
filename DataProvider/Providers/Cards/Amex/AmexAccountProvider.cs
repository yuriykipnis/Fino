using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataProvider.Providers.Cards.Amex.Dto;
using DataProvider.Providers.Interfaces;
using DataProvider.Providers.Models;
using Microsoft.CodeAnalysis.CSharp;

namespace DataProvider.Providers.Cards.Amex
{
    public class AmexAccountProvider : ICreditAccountProvider
    {
        private readonly IAmexApi _api;

        private const string _providerName = "Amex";

        public AmexAccountProvider(IAmexApi api)
        {
            _api = api;
        }

        public CreditAccount GetAccount(CreditAccountDescriptor accountDescriptor)
        {
            try
            {
                var cards = GetAllCards();
                var accountDto = GenerateAccountByAccountId(accountDescriptor, cards);
                if (accountDto == null)
                {
                    return null;
                }

                var account = GetAccountInfo(accountDto);

                DateTime now = DateTime.Now;
                var currentPeriod = new DateTime(now.Year, now.Month, 1);
                var transactions = GetAccountTransactions(accountDescriptor, cards, currentPeriod);
                account.Transactions = transactions;

                return account;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public IEnumerable<CreditAccount> GetAccounts()
        {
            IList<CreditAccount> result = new List<CreditAccount>();
            var cards = GetAllCards();
            foreach (var card in cards)
            {
                result.Add(GetAccountInfo(card));
            }
            return result;
        }

        public IEnumerable<Transaction> GetTransactions(CreditAccountDescriptor accountDescriptor, DateTime startTime, DateTime endTime)
        {
            var cards = GetAllCards();
            //var result = new BlockingCollection<Transaction>();
            var result = new ConcurrentBag<Transaction>();
            
            var period = new DateTime(startTime.Year, startTime.Month, 1);
            var periodLength = 12 * (endTime.Year - startTime.Year) + endTime.Month - startTime.Month;
            try
            {
                Parallel.For(0, periodLength, month =>
                {
                    var transactions = GetAccountTransactions(accountDescriptor, cards, period.AddMonths(month));
                    foreach (var transaction in transactions)
                    {
                        result.Add(transaction);
                    }
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return result.ToList();
        }

        private CardListInfo GenerateAccountByAccountId(CreditAccountDescriptor accountDescriptor, IList<CardListInfo> cards)
        {
            var account = cards.FirstOrDefault(a => Convert.ToString(a.CardNumber)
                .Equals(accountDescriptor?.CardNumber, StringComparison.CurrentCultureIgnoreCase));

            return account;
        }

        private CreditAccount GetAccountInfo(CardListInfo account)
        {
            var accountInfo = new CreditAccount()
            {
                Name = account.CardName,
                Club = account.Club1,
                UserName = account.UserName,
                CardNumber = Convert.ToString(account.CardNumber),
                BankAccount = $"{account.BankId}-{account.BankBranchId}-{account.BankAccountId}",
                BankName = account.BankName
            };

            return accountInfo;
        }

        private IList<Transaction> GetAccountTransactions(CreditAccountDescriptor accountDescriptor, IList<CardListInfo> cards, DateTime period)
        {
            var index = GetCardIndex(accountDescriptor, cards);
            var transactions = _api.GetTransactions(index, period.Month, period.Year);

            var result = new List<Transaction>();
            foreach (var transaction in transactions)
            {
                if (transaction.DealsInbound.Equals("yes", StringComparison.OrdinalIgnoreCase))
                {
                    var pd = transaction.FullPurchaseDate;
                    string[] date = string.IsNullOrEmpty(pd) ? new[] { "1", "1", "2000" } : pd.Split('/');
                    var voucherNumberRatz = Convert.ToInt64(transaction.VoucherNumberRatz);
                    var supplierName = transaction.SupplierName;
                    var paymentSum = Convert.ToDecimal(transaction.PaymentSum);
                    var supplierId = transaction.SupplierId;
                    var creditInfo = transaction.MoreInfo;

                    if (voucherNumberRatz != 0)
                    {
                        var purchaseDate = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), Convert.ToInt32(date[0]));
                        var paymentDate = 12 * (period.Year - purchaseDate.Year) + period.Month - purchaseDate.Month;
                        result.Add(new Transaction
                        {
                            SupplierId = supplierId,
                            Id = voucherNumberRatz,
                            PurchaseDate = purchaseDate,
                            PaymentDate = purchaseDate.AddMonths(paymentDate),
                            Description = string.IsNullOrEmpty(creditInfo) ? supplierName : $"{supplierName} - {creditInfo}",
                            ProviderName = _providerName,
                            CurrentBalance = Decimal.Zero,
                            Amount = paymentSum > 0 ? paymentSum : -1 * paymentSum,
                            Type = paymentSum > 0 ? TransactionType.Expense : TransactionType.Income
                        });
                    }
                }
                else
                {
                    var pd = transaction.FullPurchaseDateOutbound as string;
                    string[] date = string.IsNullOrEmpty(pd) ? new[] { "1", "1", "2000" } : pd.Split('/');
                    var voucherNumberRatz = Convert.ToInt64(transaction.VoucherNumberRatzOutbound);
                    var supplierName = string.IsNullOrEmpty(transaction.SupplierNameOutbound) ? "" : transaction.SupplierNameOutbound;
                    var paymentSum = Convert.ToDecimal(transaction.PaymentSumOutbound);
                    var supplierId = transaction.SupplierId;

                    if (voucherNumberRatz != 0)
                    {
                        var purchaseDate = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), Convert.ToInt32(date[0]));
                        var paymentDate = 12 * (period.Year - purchaseDate.Year) + period.Month - purchaseDate.Month;
                        result.Add(new Transaction
                        {
                            SupplierId = supplierId,
                            Id = voucherNumberRatz,
                            PurchaseDate = purchaseDate,
                            PaymentDate = purchaseDate.AddMonths(paymentDate),
                            Description = supplierName,
                            ProviderName = _providerName,
                            CurrentBalance = Decimal.Zero,
                            Amount = paymentSum,
                            Type = TransactionType.Expense
                        });
                    }
                }
            }

            FilterPullTransactions(result);

            return result;
        }

        private static void FilterPullTransactions(List<Transaction> result)
        {
            result.RemoveAll(t => t.Description.Equals("משיכת מזומנים"));
        }

        private IList<CardListInfo> GetAllCards()
        {
            var listDetails = _api.GetCards();

            var cards = new List<CardListInfo>();
            if (listDetails.Table1 != null)
            {
                cards.AddRange(listDetails.Table1);
            }

            if (listDetails.Table2 != null)
            {
                cards.AddRange(listDetails.Table2);
            }
            
            return cards;
        }
        
        private long GetCardIndex(CreditAccountDescriptor accountDescriptor, IList<CardListInfo> cards)
        {
            int index = 0;
            var card = cards[0];
            while (!Convert.ToString(card.CardNumber).Equals(accountDescriptor?.CardNumber) && cards.Count > index)
            {
                index++;
                card = cards[index];
            }
            return index;
        }

        public void Dispose()
        {
            _api.Dispose();
        }
    }
}
