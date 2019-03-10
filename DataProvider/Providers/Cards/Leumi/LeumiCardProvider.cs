using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataProvider.Providers.Interfaces;
using DataProvider.Providers.Models;

namespace DataProvider.Providers.Cards.Leumi
{
    public class LeumiCardProvider : ICreditCardProvider
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CreditCard> GetCards()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CreditCard> GetCardsWithTransactions(List<CreditCardDescriptor> creditCardDescriptor, DateTime startDate, DateTime endDate,
            bool includeDeatils = false)
        {
            throw new NotImplementedException();
        }
    }
}
