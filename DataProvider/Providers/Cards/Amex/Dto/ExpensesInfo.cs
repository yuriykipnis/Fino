using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProvider.Providers.Cards.Amex.Dto
{
    public class ExpensesInfo
    {
        public IEnumerable<CardTransaction> Transactions { get; set; }
        public Decimal NextCharge { get; set; }

    }
}
