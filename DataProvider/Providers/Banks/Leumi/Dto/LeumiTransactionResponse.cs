using System;
using DataProvider.Providers.Banks.Leumi.Dto.Converters;
using DataProvider.Providers.Models;
using Newtonsoft.Json;

namespace DataProvider.Providers.Banks.Leumi.Dto
{
    public class LeumiTransactionResponse
    {
        public TransactionType Type { get; set; }

        public DateTime PurchaseDate { get; set; }

        public DateTime PaymentDate { get; set; }

        [JsonConverter(typeof(BitStringConverter))]
        public String Description { get; set; }

        public Decimal Amount { get; set; }

        public Decimal CurrentBalance { get; set; }

        public string SupplierId { get; set; }
    }
}
