using System;
using System.Collections.Generic;

namespace DataProvider.Providers.Banks.Hapoalim.Dto
{
    public class HapoalimMortgagesResponse
    {
        public Decimal ArrearsAmount { get; set; }
        public IEnumerable<MortgageData> Data { get; set; }
        public string FormattedValidityDate { get; set; }
        public string PartyId { get; set; }
        public Decimal PaymentBalance { get; set; }
        public Decimal RevaluedBalance { get; set; }
        public long ValidityDate { get; set; }

        public class MortgageData
        {
            public string AccountNumber { get; set; }
            public Decimal ArrearsAmount { get; set; }
            public int AssestInsuranceAmount { get; set; }
            public string AssetInsuranceCompanyName { get; set; }
            public Decimal AssetInsurancePaymentAmount { get; set; }
            public string AssetInsuranceTypeDescription { get; set; }
            public int BankNumber { get; set; }
            public int BranchNumber { get; set; }
            public long CalculatedEndDate { get; set; }
            public string CollectionMethodCode { get; set; }
            public int DebitAccountsQuantity { get; set; }
            public long ExecutingDate { get; set; }
            public string FormattedCalculatedEndDate { get; set; }
            public string FormattedExecutingDate { get; set; }
            public string FormattedStartDate { get; set; }
            public Decimal InsuranceAssetsArea { get; set; }
            public string InterestTypeDescription { get; set; }
            public string LifeInsuranceCompanyName { get; set; }
            public string LifeInsuranceTypeDescription { get; set; }
            public string LinkageTypeDescription { get; set; }

            public string MortgageLoanSerialId { get; set; }
            public Decimal PaymentAmount { get; set; }
            public Decimal PrepaymentCommissionTotalAmount { get; set; }
            public string ProductLabel { get; set; }
            public Decimal RevaluedBalance { get; set; }
            public string StartDate { get; set; }

            public IEnumerable<SubMortgageData> SubLoanData { get; set; }

            public class SubMortgageData
            {
                public Decimal AmountAndLinkageOfInterestDeferred { get; set; }
                public Decimal AmountAndLinkageOfPrincipal { get; set; }
                public Decimal BasicIndexValue { get; set; }
                public long CalculatedEndDate { get; set; }
                public Decimal DeferredInterestAmount { get; set; }
                public Decimal DeferredInterestLinkageAmount { get; set; }
                public long EndDate { get; set; }
                public long ExecutingDate { get; set; }

                public string FormattedCalculatedEndDate { get; set; }
                public string FormattedEndDate { get; set; }
                public string FormattedExecutingDate { get; set; }
                public string FormattedNextExitDate { get; set; }
                public string FormattedStartDate { get; set; }

                public Decimal InterestAmount { get; set; }
                public Decimal InterestAndLinkageTotalAmount { get; set; }
                public Decimal InterestLinkageAmount { get; set; }
                public long NextExitDate { get; set; }

                public Decimal PrincipalAndInterestAndInterestDeferredTotalAmount { get; set; }
                public Decimal PrincipalBalanceAmount { get; set; }
                public Decimal PrincipalLinkageAmount { get; set; }
                public Decimal RevaluedBalance { get; set; }
                public long StartDate { get; set; }
                public Decimal SubLoansPrincipalAmount { get; set; }
                public long SubLoansSerialId { get; set; }

                public Decimal SumOfPrincipalAndCurrentLinkageAndDeferredLinkageAmount { get; set; }
                public Decimal ValidityInterestRate { get; set; }
            }
        }
    }
}
