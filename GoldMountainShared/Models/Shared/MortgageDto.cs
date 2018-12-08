using System;
using System.Collections.Generic;
using System.Text;
using GoldMountainShared.Storage.Documents;

namespace GoldMountainShared.Models.Shared
{
    public class MortgageDto
    {
        public String Id { get; set; } = String.Empty;
        public String LoanId { get; set; }

        public DateTime StartDate { get; set; } = DateTime.MinValue;
        public DateTime EndDate { get; set; } = DateTime.MinValue;

        public Decimal OriginalAmount { get; set; } = 0;
        public Decimal DeptAmount { get; set; } = 0;
        public Decimal InterestAmount { get; set; } = 0;

        public Decimal PrepaymentCommission { get; set; } = 0;
        public DateTime NextExitDate { get; set; } = DateTime.MinValue;

        public Decimal InterestRate { get; set; } = 0;
        public String InterestType { get; set; } = String.Empty;
        public String LinkageType { get; set; } = String.Empty;
        public String InsuranceCompany { get; set; } = String.Empty;

        public MortgageAsset Asset { get; set; }

        public class MortgageAsset
        {
            public String CityName { get; set; } = String.Empty;
            public String StreetName { get; set; } = String.Empty;
            public String BuildingNumber { get; set; } = String.Empty;
            public String PartyFirstName { get; set; } = String.Empty;
            public String PartyLastName { get; set; } = String.Empty;
        }
    }
}
