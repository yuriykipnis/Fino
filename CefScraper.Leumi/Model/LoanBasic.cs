using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefScraper.Leumi.Model.Converters;
using Newtonsoft.Json;

namespace CefScraper.Leumi.Model
{
    public class LoanBasic
    {
        [JsonConverter(typeof(StringBitConverter))]
        public String LoanId { get; set; }

        [JsonConverter(typeof(DateConverter))]
        public DateTime StartDate { get; set; } = DateTime.MinValue;

        [JsonConverter(typeof(DateConverter))]
        public DateTime EndDate { get; set; } = DateTime.MinValue;

        [JsonConverter(typeof(DecimalConverter))]
        public Decimal OriginalAmount { get; set; } = 0;

        [JsonConverter(typeof(DecimalConverter))]
        public Decimal DeptAmount { get; set; } = 0;

        [JsonConverter(typeof(DecimalConverter))]
        public Decimal InterestRate { get; set; } = 0;

        [JsonConverter(typeof(StringBitConverter))]
        public String Type { get; set; } = String.Empty;

        [JsonConverter(typeof(DecimalConverter))]
        public Decimal NextPrepayment { get; set; } = 0;

        [JsonConverter(typeof(DateConverter))]
        public DateTime NextPaymentDate { get; set; } = DateTime.MinValue;
    }
}
