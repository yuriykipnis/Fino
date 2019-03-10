using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldMountainShared.Storage.Converters;
using Newtonsoft.Json;

namespace DataProvider.Providers.Models
{
    public class CreditCardTransaction
    {
        public String Id { get; set; }

        public Decimal PaymentAmount { get; set; }
        public String PaymentCurrency { get; set; }
        public DateTime PaymentDate { get; set; }

        public Decimal DealAmount { get; set; }
        public String DealCurrency { get; set; }
        public DateTime DealDate { get; set; }

        public String Description { get; set; }
        public String Notes { get; set; }

        public String SupplierAddress { get; set; }
        public String DealSector { get; set; }
    }
}
