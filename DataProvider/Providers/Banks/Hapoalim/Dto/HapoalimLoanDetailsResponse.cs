using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProvider.Providers.Banks.Hapoalim.Dto
{
    public class HapoalimLoanDetailsResponse
    {
        public Decimal ActualPrincipalBalance { get; set; }
        public Decimal AmortizationSchedulePrincipalBalance { get; set; }
        public Decimal ArrearsAmount { get; set; }
        public string ArrearsExistanceSwitch { get; set; }
        public Decimal ArrearsInterestCorrectionAmount { get; set; }
        public Decimal ArrearsInterestPaymentBalance { get; set; }
        public Decimal ArrearsInterestPercentage { get; set; }
        public Decimal ArrearsLinkageAmount { get; set; }
        public string ArrearsStartDate { get; set; }
        public Decimal ArrearTotalAmount { get; set; }
        public int CreditSerialNumber { get; set; }
        public Decimal CurrentInterestPercent { get; set; }
        public Decimal DebitInterestCorrectionAmount { get; set; }
        public string FormattedArrearsStartDate { get; set; }
        public string FormattedInterestPercentUpdatingDate { get; set; }
        public string FormattedLoanEndDate { get; set; }
        public string FormattedValueDate { get; set; }
        public Decimal InterestAmount { get; set; }
        public int InterestNextPaymentNumber { get; set; }
        public int InterestPaymentsNumberBalance { get; set; }
        public long InterestPercentUpdatingDate { get; set; }
        public string InterestTypeDescription { get; set; }
        public string LinkageTypeDescription { get; set; }
        public Decimal LoanBalanceAmount { get; set; }
        public long LoanEndDate { get; set; }
        public int OriginalInterestPaymentsNumber { get; set; }
        public Decimal OriginalInterestPercent { get; set; }
        public int OriginalPrincipalPaymentsNumber { get; set; }
        public string PartyExplanationText { get; set; }
        public Decimal PaymentWithoutArrears { get; set; }
        public int PrincipalNextPaymentNumber { get; set; }
        public int PrincipalPaymentsNumberBalance { get; set; }
        public string UnitedCreditTypeCode { get; set; }
        public long ValueDate { get; set; }
    }
}
