using System;
using System.Collections.Generic;
using System.Text;

namespace GoldMountainShared.Models.Shared
{
    public class LoanDto
    {
        public String Id { get; set; } = String.Empty;
        public String LoanId { get; set; }

        public DateTime StartDate { get; set; } = DateTime.MinValue;
        public DateTime EndDate { get; set; } = DateTime.MinValue;
        public DateTime NextPaymentDate { get; set; } = DateTime.MinValue;

        public Double OriginalAmount { get; set; } = 0;
        public Double DeptAmount { get; set; } = 0;
        public Double LastPaymentAmount { get; set; } = 0;
        public Double PrepaymentCommission { get; set; } = 0;

        public String InterestType { get; set; } = String.Empty;
        public String LinkageType { get; set; } = String.Empty;
        public String InsuranceCompany { get; set; } = String.Empty;

        public IList<SubLoan> SubLoans { get; set; } = new List<SubLoan>();

        public class SubLoan
        {
            public String Id { get; set; }

            public Double OriginalAmount { get; set; } = 0;
            public Double PrincipalAmount { get; set; } = 0;
            public Double InterestAmount { get; set; } = 0;
            public Double DebtAmount { get; set; } = 0;
            public DateTime NextExitDate { get; set; } = DateTime.MinValue;

            public DateTime StartDate { get; set; } = DateTime.MinValue;
            public DateTime EndDate { get; set; } = DateTime.MinValue;
            public Double InterestRate { get; set; } = 0;
        }
    }
}
