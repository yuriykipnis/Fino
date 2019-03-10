using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataProvider.Providers.Cards.Amex.Dto;

namespace DataProvider.Providers.Cards.Amex
{
    public interface IAmexApi : IDisposable
    {
        IEnumerable<AmexCardInfo> GetCards();
        IEnumerable<CardChargeResponse> GetBankDebit(string accountNumber, string cardNumber, int year, int month);
        IEnumerable<Dto.AmexCardTransaction> GetTransactions(int cardIndex, int year, int month);
        DealDetails GetTransactionDetails(int cardIndex, string paymentDate, Boolean isInbound, string transactionId);
    }
}
