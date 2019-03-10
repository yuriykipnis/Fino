using System;
using System.Collections.Generic;

namespace DataProvider.Providers.Cards.Amex.Dto
{
    public class AmexExpensesInfo
    {
        public IEnumerable<AmexCardTransaction> Transactions { get; set; }
        public Decimal NextCharge { get; set; }

    }
}
