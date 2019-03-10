using System;
using System.Collections.Generic;
using DataProvider.Providers.Models;

namespace DataProvider.Providers.Interfaces
{
    public interface ICreditCardProvider : IAccountProvider
    {
        IEnumerable<CreditCard> GetCards();
        IEnumerable<CreditCard> GetCardsWithTransactions(List<CreditCardDescriptor> creditCardDescriptor,
                                    DateTime startDate, DateTime endDate, bool includeDeatils = false);
    }
}
