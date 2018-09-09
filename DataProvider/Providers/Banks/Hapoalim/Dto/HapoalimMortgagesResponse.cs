using System;
using System.Collections.Generic;

namespace DataProvider.Providers.Banks.Hapoalim.Dto
{
    public class HapoalimMortgagesResponse
    {
        public Double ArrearsAmount { get; set; }
        public IEnumerable<MortgageData> Data { get; set; }
        public string FormattedValidityDate { get; set; }
        public string PartyId { get; set; }
        public Double PaymentBalance { get; set; }
        public Double RevaluedBalance { get; set; }
        public long ValidityDate { get; set; }

        public class MortgageData
        {
            public string AccountNumber { get; set; }
            public Double ArrearsAmount { get; set; }
            public int AssestInsuranceAmount { get; set; }
            public string AssetInsuranceCompanyName { get; set; }
            public Double AssetInsurancePaymentAmount { get; set; }
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
            public Double InsuranceAssetsArea { get; set; }
            public string InterestTypeDescription { get; set; }
            public string LifeInsuranceCompanyName { get; set; }
            public string LifeInsuranceTypeDescription { get; set; }
            public string LinkageTypeDescription { get; set; }

            public string MortgageLoanSerialId { get; set; }
            public Double PaymentAmount { get; set; }
            public Double PrepaymentCommissionTotalAmount { get; set; }
            public string ProductLabel { get; set; }
            public Double RevaluedBalance { get; set; }
            public string StartDate { get; set; }

            public IEnumerable<SubMortgageData> SubLoanData { get; set; }

            public class SubMortgageData
            {
                public Double AmountAndLinkageOfInterestDeferred { get; set; }
                public Double AmountAndLinkageOfPrincipal { get; set; }
                public Double BasicIndexValue { get; set; }
                public long CalculatedEndDate { get; set; }
                public Double DeferredInterestAmount { get; set; }
                public Double DeferredInterestLinkageAmount { get; set; }
                public long EndDate { get; set; }
                public long ExecutingDate { get; set; }

                public string FormattedCalculatedEndDate { get; set; }
                public string FormattedEndDate { get; set; }
                public string FormattedExecutingDate { get; set; }
                public string FormattedNextExitDate { get; set; }
                public string FormattedStartDate { get; set; }

                public Double InterestAmount { get; set; }
                public Double InterestAndLinkageTotalAmount { get; set; }
                public Double InterestLinkageAmount { get; set; }
                public long NextExitDate { get; set; }

                public Double PrincipalAndInterestAndInterestDeferredTotalAmount { get; set; }
                public Double PrincipalBalanceAmount { get; set; }
                public Double PrincipalLinkageAmount { get; set; }
                public Double RevaluedBalance { get; set; }
                public long StartDate { get; set; }
                public Double SubLoansPrincipalAmount { get; set; }
                public long SubLoansSerialId { get; set; }

                public Double SumOfPrincipalAndCurrentLinkageAndDeferredLinkageAmount { get; set; }
                public Double ValidityInterestRate { get; set; }
            }
        }
    }
}
