using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataProvider.Providers.Banks.Leumi.Dto.Converters;
using Newtonsoft.Json;

namespace DataProvider.Providers.Banks.Leumi.Dto
{
    public class LeumiMortgageResponse
    {
        [JsonConverter(typeof(BitStringConverter))]
        public String LoanId { get; set; } = String.Empty;

        public DateTime StartDate { get; set; } = DateTime.MinValue;
        public DateTime EndDate { get; set; } = DateTime.MinValue;

        public Decimal OriginalAmount { get; set; } = 0;
        public Decimal DeptAmount { get; set; } = 0;
        public Decimal InterestAmount { get; set; } = 0;

        public Decimal LastPaymentAmount { get; set; } = 0;

        public Decimal PrepaymentCommission { get; set; } = 0;
        public DateTime NextExitDate { get; set; } = DateTime.MinValue;

        public Decimal InterestRate { get; set; } = 0;
        [JsonConverter(typeof(BitStringConverter))]
        public String InterestType { get; set; } = String.Empty;
        [JsonConverter(typeof(BitStringConverter))]
        public String LinkageType { get; set; } = String.Empty;
    }
}
