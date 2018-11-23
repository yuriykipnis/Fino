using System;
using System.Collections.Generic;

namespace DataProvider.Providers.Models
{
    public class Mortgage
    {
        public String LoanId { get; set; }
        public DateTime StartDate { get; set; } = DateTime.MinValue;
        public DateTime EndDate { get; set; } = DateTime.MinValue;

        public Decimal OriginalAmount { get; set; } = 0;
        public Decimal DeptAmount { get; set; } = 0;

        public Decimal PrepaymentCommission { get; set; } = 0;
        public DateTime NextExitDate { get; set; } = DateTime.MinValue;

        public Decimal InterestRate { get; set; } = 0;
        public String InterestType { get; set; } = String.Empty;
        public String LinkageType { get; set; } = String.Empty;
        public String InsuranceCompany { get; set; } = String.Empty;

        public MortgageAsset Asset { get; set; }

        //public class SubLoan
        //{
        //    public String Id { get; set; }

        //    public Decimal OriginalAmount { get; set; } = 0;
        //    public Decimal PrincipalAmount { get; set; } = 0;
        //    public Decimal InterestAmount { get; set; } = 0;
        //    public Decimal DebtAmount { get; set; } = 0;
        //    public DateTime NextExitDate { get; set; } = DateTime.MinValue;

        //    public DateTime StartDate { get; set; } = DateTime.MinValue;
        //    public DateTime EndDate { get; set; } = DateTime.MinValue;
        //    public Decimal InterestRate { get; set; } = 0;
        //}
    }
}
