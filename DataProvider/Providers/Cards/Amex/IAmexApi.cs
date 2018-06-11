using System;
using System.Collections.Generic;
using DataProvider.Providers.Cards.Amex.Dto;

namespace DataProvider.Providers.Cards.Amex
{
    public interface IAmexApi : IDisposable
    {
        CardListDeatils GetCards();
        IEnumerable<CardTransaction> GetTransactions(long cardIndex, int month, int year);
    }
}
