using System;
using System.Collections.Generic;
using System.Linq;

namespace DataProvider.Providers.Cards.Cal.Dto
{
    public class CalTransactionDetailsResponse
    {   
        public TransactionDetailsData Data { get; set; }
        public CalStatusResponse Response { get; set; }
    }

    public class TransactionDetailsData
    {
        public TransactionDetailsMerchant MerchantDetails { get; set; }
        public IEnumerable<TransactionDetailsFields> DataFields { get; set; }
    }

    public class TransactionDetailsMerchant
    {
        public String Address { get; set; }
        public String FaxNumber { get; set; }
        public String Id { get; set; }
        public String Name { get; set; }
        public String PhoneNumber { get; set; }
        public String SectorCode { get; set; }
        public String SectorName { get; set; }
        public String Type { get; set; }
    }

    public class TransactionDetailsFields
    {
        public Boolean IsMoney { get; set; }
        public String Name { get; set; }
        public String Value { get; set; }
    }
}
