using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProvider.Providers.Cards.Amex.Dto
{
    public class DashboardMonthResponse
    {
        public AmexHeaderResponse Header { get; set; }
        public DashboardMonthBeanResponse DashboardMonthBean { get; set; }
    }

    public class DashboardMonthBeanResponse
    {
        public IEnumerable<CardChargeResponse> CardsCharges { get; set; }
        public Decimal TotalDebitShekel { get; set; }
    }

    public class CardChargeResponse
    {
        public String BillingDate { get; set; }
        public Decimal BillingSumDollar { get; set; }
        public Decimal BillingSumEuro { get; set; }
        public Decimal BillingSumSekel { get; set; }
        public int CardIndex { get; set; }
        public String CardNumber { get; set; }
    }
}
