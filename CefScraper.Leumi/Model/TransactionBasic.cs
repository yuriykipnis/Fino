using System;
using System.Linq;
using CefScraper.Leumi.Model.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CefScraper.Leumi.Model
{
    public class TransactionBasic
    {
        public long Id { get; set; }

        [JsonConverter(typeof(TransactionTypeConverter))]
        public TransactionType Type { get; set; }

        [JsonConverter(typeof(DateConverter))]
        public DateTime PurchaseDate { get; set; }

        [JsonConverter(typeof(DateConverter))]
        public DateTime PaymentDate { get; set; }

        [JsonConverter(typeof(StringBitConverter))]
        public String Description { get; set; }

        [JsonConverter(typeof(DecimalConverter))]
        public Decimal Amount { get; set; }

        [JsonConverter(typeof(DecimalConverter))]
        public Decimal CurrentBalance { get; set; }

        public string SupplierId { get; set; }
    }

    public enum TransactionType { Income, Expense, None };
}
