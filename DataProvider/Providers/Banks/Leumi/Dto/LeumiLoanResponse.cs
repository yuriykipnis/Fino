using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataProvider.Providers.Banks.Leumi.Dto.Converters;
using Newtonsoft.Json;

namespace DataProvider.Providers.Banks.Leumi.Dto
{
    public class LeumiLoanResponse
    {
        [JsonConverter(typeof(BitStringConverter))]
        public String LoanId { get; set; }

        public DateTime StartDate { get; set; } = DateTime.MinValue;

        public DateTime EndDate { get; set; } = DateTime.MinValue;

        public Decimal OriginalAmount { get; set; } = 0;

        public Decimal DeptAmount { get; set; } = 0;

        public Decimal InterestRate { get; set; } = 0;

        [JsonConverter(typeof(BitStringConverter))]
        public String Type { get; set; } = String.Empty;

        public Decimal NextPrepayment { get; set; } = 0;

        public DateTime NextPaymentDate { get; set; } = DateTime.MinValue;
    }
}
