using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataProvider.Providers.Cards.Cal.Dto;

namespace DataProvider.Providers.Cards.Cal
{
    public interface ICalApi : IDisposable
    {
        CalGetCardsResponse GetCards();
        IEnumerable<CalTransactionResponse> GetTransactions(string cardId, DateTime startDate, DateTime endDate);
        IEnumerable<CalBankDebit> GetBankDebits(string bankAccountId, string cardId, DateTime startDate, DateTime endDate);
        CalTransactionDetailsResponse GetTransactionDetails(string transactionId, int numerator);
    }
}
