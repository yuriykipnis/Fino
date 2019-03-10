using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProvider.Providers.Cards.Amex.Dto
{
    public class TransactionDetailsResponse
    {
        public AmexHeaderResponse Header { get; set; }
        public DealDetails PirteyIska_204Bean{ get; set; }
    }

    public class DealDetails
    {
        public String Address { get; set; }
        public String PurchaseTime { get; set; }
        public String Sector { get; set; }
        public String SuplierName { get; set; }
        public String PhoneNumber { get; set; }

        public String IsInbound { get; set; }
        public String City { get; set; }
        public String SuplierNameIsracard { get; set; }

        public String CommissionNetValue { get; set; }
        public String DollarTransferCommissionPercentage { get; set; }
        public String DollarTransferRate { get; set; }
        public String SumDollar { get; set; }
        public String DealSum { get; set; }
        public String DealSumOutbound { get; set; }
        public String CurrencyId { get; set; }
        public String CurrencyIdForCharge { get; set; }
    }
}
