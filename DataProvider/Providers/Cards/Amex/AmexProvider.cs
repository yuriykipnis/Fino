using System;
using System.Collections.Generic;
using DataProvider.Providers.Models;
using System.Linq;
using System.Threading.Tasks;
using DataProvider.Providers.Cards.Amex.Dto;
using DataProvider.Providers.Interfaces;

namespace DataProvider.Providers.Cards.Amex
{
    public class AmexProvider : IDisposable, ICreditCardProvider
    {
        private const string ProviderName = "Amex";
        private readonly IAmexApi _api;

        public AmexProvider(IAmexApi api)
        {
            _api = api;
        }

        public IEnumerable<CreditCard> GetCards()
        {
            var result = new List<CreditCard>();
            var cards = _api.GetCards();
            foreach (var card in cards)
            {
                result.Add(GenerateCredtiCard(card));
            }

            return result;
        }
        
        public IEnumerable<CreditCard> GetCardsWithTransactions(List<CreditCardDescriptor> creditCardDescriptor,
                                                                DateTime startDate, DateTime endDate, bool includeDeatils = false)
        {
            var result = new List<CreditCard>();
            var cardsInfo = _api.GetCards();

            var cards = cardsInfo.Where(c =>
            {
                return creditCardDescriptor.Any(d => d.CardId == GenerateCardId(c));
            });

            foreach (var card in cards)
            {
                var basicDebits = GenerateBasicDebits(card, startDate, endDate);
                var allDebits = GenerateDebitsWithTransactions(card, basicDebits, includeDeatils);

                var newCard = GenerateCredtiCard(card);
                newCard.Debits = allDebits;
                result.Add(newCard);
            }
            
            return result;
        }
        
        private List<CreditCardDebitPeriod> GenerateDebitsWithTransactions(AmexCardInfo card, List<CreditCardDebitPeriod> debits, bool includeDeatils)
        {
            var result = new List<CreditCardDebitPeriod>();

            foreach (var debit in debits)
            {
                var transactions = _api.GetTransactions(card.CardIndex, debit.Date.Year, debit.Date.Month).ToList();

                var transactionalDebits = FetchInternalDebits(card, debit, transactions, includeDeatils);

                debit.Transactions = GenerateCardTransactions(transactions, debit.Date);

                if (includeDeatils)
                {
                    EnrichTransactionsByDetails(card, debit, transactions);
                }
                

                result.AddRange(transactionalDebits);
            }

            result.AddRange(debits);
            return result;
        }

        private List<CreditCardDebitPeriod> GenerateBasicDebits(AmexCardInfo card, DateTime startDate, DateTime endDate)
        {
            var result = new List<CreditCardDebitPeriod>();
            for (DateTime date = startDate; date < endDate.AddMonths(1); date = date.AddMonths(1))
            {
                var bankCharges = _api.GetBankDebit(card.BankAccountId, card.CardNumber, date.Year, date.Month);
                var bankDebits = GenerateDebitPeriods(card, bankCharges);
                result.AddRange(bankDebits);
            }

            return result;
        }

        private void EnrichTransactionsByDetails(AmexCardInfo card, CreditCardDebitPeriod debit, List<AmexCardTransaction> transactions)
        {
            var tasks = CreateDetailsEnrichmentTasks(card.CardIndex, debit, transactions);

            foreach (var task in tasks)
            {
                task.Start();
            }

            Task.WaitAll(tasks.ToArray());
        }

        private List<Task> CreateDetailsEnrichmentTasks(int cardIndex, CreditCardDebitPeriod debit, List<AmexCardTransaction> dtoTransactions)
        {
            var result = new List<Task>();
            var period = $"{debit.Date.Month}{debit.Date.Year}";

            foreach (var trs in debit.Transactions)
            {
                var trsToEnrich = dtoTransactions.FirstOrDefault(t => trs.Id == t.VoucherNumberRatz || trs.Id == t.VoucherNumberRatzOutbound);
                if (trsToEnrich == null) continue;
                
                var task = new Task((p) =>
                {
                    try
                    {
                        TransactionEnrichmentParameters parameters = (TransactionEnrichmentParameters) p;
                        var isInbound = !string.IsNullOrEmpty(trsToEnrich.DealsInbound) &&
                                        !trsToEnrich.DealsInbound.Equals("no", StringComparison.CurrentCultureIgnoreCase);

                        var details = _api.GetTransactionDetails(cardIndex, period,
                            isInbound, parameters.Transaction.Id);

                        parameters.Transaction.DealSector = (details?.Sector ?? String.Empty).Trim();
                        parameters.Transaction.SupplierAddress = (details?.Address ?? String.Empty).Trim();
                    }
                    catch (Exception exp)
                    {
                    }
                }, new TransactionEnrichmentParameters
                {
                    RowTransaction = trsToEnrich,
                    Transaction= trs
                });

                result.Add(task);
            }

            return result;
        }

        private List<CreditCardDebitPeriod> FetchInternalDebits(AmexCardInfo card, CreditCardDebitPeriod debit, IList<AmexCardTransaction> transactions, bool includeDeatils)
        {
            var result = new List<CreditCardDebitPeriod>();

            for (int i = 0; i < transactions.Count; i++)
            {
                var trs = transactions[i];
                if (!String.IsNullOrEmpty(trs.DealsInbound) && trs.VoucherNumber == null)
                {
                    if (trs.DealSumType == "P" && trs.MoreInfo == "חיוב במועד הבא")
                    {
                        transactions.RemoveAt(i);
                        transactions.RemoveAt(i-1);
                        i-=2; 
                    }
                    else if (trs.DealSumType == "T" && trs.DealsInbound == "NO")
                    {
                        transactions.RemoveAt(i);
                        i--;
                    }
                    else if (trs.DealSumType == "T" && trs.DealsInbound == "yes")
                    {
                        var newDebit = GenerateDebitPeriodForTransaction(card, debit, transactions[i-1]);
                        var debitTrs = GenerateCardTransaction(transactions[i - 1], newDebit.Date);
                        
                        newDebit.Transactions.Add(debitTrs);
                        result.Add(newDebit);

                        if (includeDeatils)
                        {
                            EnrichTransactionsByDetails(card, newDebit, transactions.ToList());
                        }

                        transactions.RemoveAt(i);
                        transactions.RemoveAt(i-1);
                        i-=2;
                    }
                    else if (trs.DealSumType == "1")
                    {
                        transactions.RemoveAt(i);
                        i--;
                    }
                }
                else if (String.IsNullOrEmpty(trs.DealsInbound))
                {
                    if (trs.DealSumOutbound == null && !trs.FullSupplierNameOutbound.Equals("CASH ADVANCE FEE"))
                    {
                        transactions.RemoveAt(i);
                        i--;
                    }
                    else if (Convert.ToDecimal(trs.DealSumOutbound) < 0)
                    {
                        var newDebit = GenerateDebitPeriodForTransaction(card, debit, trs);
                        var debitTrs = GenerateCardTransaction(trs, newDebit.Date);

                        newDebit.Transactions.Add(debitTrs);
                        result.Add(newDebit);

                        if (includeDeatils)
                        {
                            EnrichTransactionsByDetails(card, newDebit, transactions.ToList());
                        }

                        transactions.RemoveAt(i);
                        i--;
                    }
                }
            }

            return result;
        }

        private static CreditCardDebitPeriod GenerateDebitPeriodForTransaction(AmexCardInfo card, CreditCardDebitPeriod debit, AmexCardTransaction transaction)
        {
            return new CreditCardDebitPeriod
            {
                CardId = GenerateCardId(card),
                CardLastDigits = card.CardNumber,
                Date = debit.Date,

                Amount = String.IsNullOrEmpty(transaction.DealSumOutbound) 
                    ? Convert.ToDecimal(transaction.PaymentSum)
                    : Convert.ToDecimal(transaction.DealSumOutbound),

                Transactions = new List<CreditCardTransaction>()
            };
        }

        private IList<CreditCardTransaction> GenerateCardTransactions(IEnumerable<AmexCardTransaction> transactions, DateTime debitDate)
        {
            var result = new List<CreditCardTransaction>();

            foreach (var transaction in transactions)
            {
                result.Add(GenerateCardTransaction(transaction, debitDate));
            }

            return result;
        }

        private static CreditCardTransaction GenerateCardTransaction(AmexCardTransaction transaction, DateTime debitDate)
        {
            return new CreditCardTransaction
            {
                Id = !String.IsNullOrEmpty(transaction.DealsInbound)? 
                        transaction.VoucherNumberRatz :
                        transaction.VoucherNumberRatzOutbound,

                DealDate = !String.IsNullOrEmpty(transaction.DealsInbound)
                    ? AutoMapper.Mapper.Map<DateTime>(transaction.FullPurchaseDate) 
                    : AutoMapper.Mapper.Map<DateTime>(transaction.FullPurchaseDateOutbound),

                DealAmount = !String.IsNullOrEmpty(transaction.DealsInbound)
                    ? Convert.ToDecimal(transaction.DealSum)
                    : Convert.ToDecimal(transaction.DealSumOutbound),

                DealCurrency = !String.IsNullOrEmpty(transaction.DealsInbound)
                    ? transaction.CurrencyId
                    : transaction.CurrentPaymentCurrency,

                PaymentDate = transaction.PaymentSum == transaction.DealSum
                    ? !String.IsNullOrEmpty(transaction.DealsInbound)
                        ? AutoMapper.Mapper.Map<DateTime>(transaction.FullPurchaseDate)
                        : AutoMapper.Mapper.Map<DateTime>(transaction.FullPaymentDate)
                    : AutoMapper.Mapper.Map<DateTime>(debitDate),

                PaymentAmount = !String.IsNullOrEmpty(transaction.DealsInbound)
                    ? Convert.ToDecimal(transaction.PaymentSum)
                    : Convert.ToDecimal(transaction.PaymentSumOutbound),

                PaymentCurrency = transaction.CurrencyId,

                Description = !String.IsNullOrEmpty(transaction.DealsInbound)
                    ? transaction.SupplierName
                    : transaction.SupplierNameOutbound,

                Notes = transaction.MoreInfo
            };
        }
     
        private IEnumerable<CreditCardDebitPeriod> GenerateDebitPeriods(AmexCardInfo card, IEnumerable<CardChargeResponse> bankCharges)
        {
            var result = new List<CreditCardDebitPeriod>();
            foreach (var bankCharge in bankCharges)
            {
                result.Add(GenerateDebitPeriod(card, bankCharge));
            }

            return result;
        }

        private static CreditCardDebitPeriod GenerateDebitPeriod(AmexCardInfo card, CardChargeResponse bankCharge)
        {
            return new CreditCardDebitPeriod
            {
                CardId = GenerateCardId(card),
                CardLastDigits = bankCharge.CardNumber,
                Date = AutoMapper.Mapper.Map<DateTime>(bankCharge.BillingDate),
                Amount = bankCharge.BillingSumSekel,
                Transactions = new List<CreditCardTransaction>()
            };
        }

        private CreditCard GenerateCredtiCard(AmexCardInfo card)
        {
            var account = new CreditCardBankAccount
            {
                Id = GenerateBankAccountId(card),
                BankName = card.BankName,
                BankCode = card.BankId,
                BankBranchNumber = card.BankBranchId,
                AccountNumber = card.BankAccountId
            };

            var splitName = card.UserName.Split(' ');
            var result = new CreditCard
            {
                Id = GenerateCardId(card),
                CardName = card.CardName,
                LastDigits = card.CardNumber,
                HolderId = card.HolderId,
                OwnerFirstName = splitName[1],
                OwnerLastName = splitName[0],
                TypeDescription = card.Club1,
                CardAccount = account
            };

            return result;
        }

        private static string GenerateBankAccountId(AmexCardInfo card)
        {
            return $"{card.BankId}-{card.BankBranchId}-{card.BankAccountId}";
        }

        private static string GenerateCardId(AmexCardInfo card)
        {
            return $"{card.HolderId}-{card.CardNumber}";
        }

        public void Dispose()
        {
            _api.Dispose();
        }

        private class TransactionEnrichmentParameters
        {
            //public Boolean IsInbound { get; set; }
            //public String Voucher { get; set; }

            public Dto.AmexCardTransaction RowTransaction { get; set; }
            public CreditCardTransaction Transaction { get; set; }
        }
    }

    
}
