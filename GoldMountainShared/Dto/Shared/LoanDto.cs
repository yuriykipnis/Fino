using System;

namespace GoldMountainShared.Dto.Shared
{
    public class LoanDto
    {
        public String LoanId { get; set; }
        public DateTime StartDate { get; set; } = DateTime.MinValue;
        public DateTime EndDate { get; set; } = DateTime.MinValue;

        public Decimal OriginalAmount { get; set; } = 0;
        public Decimal DeptAmount { get; set; } = 0;
        public Decimal InterestRate { get; set; } = 0;

        public int NumberOfPrincipalPayments { get; set; } = 0;
        public int NumberOfInterestPayments { get; set; } = 0;
        public int NextPrincipalPayment { get; set; } = 0;
        public int NextInterestPayment { get; set; } = 0;

        public Decimal NextPrepayment { get; set; } = 0;
        public DateTime NextPaymentDate { get; set; } = DateTime.MinValue;
    }
}
