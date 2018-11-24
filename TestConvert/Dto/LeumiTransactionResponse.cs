using System;
using CefScraper.Leumi.Model;
using Newtonsoft.Json;
using TestConvert.Dto.Converters;

namespace TestConvert.Dto
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
