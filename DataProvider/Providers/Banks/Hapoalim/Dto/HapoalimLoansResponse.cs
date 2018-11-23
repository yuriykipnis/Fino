using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProvider.Providers.Banks.Hapoalim.Dto
{
    public class HapoalimLoansResponse
    {
        public string AccountNumber { get; set; }
        public int BranchNumber { get; set; }
        public string ValidityDate { get; set; }
        public string FormattedValidityDate { get; set; }
        public Decimal LoansDebtAmount { get; set; }
        public Decimal NextPaymentAmountInCurrentMonth { get; set; }

        public IEnumerable<LoanData> Data { get; set; }

        public class LoanData
        {
            public Decimal ArrearTotalAmount { get; set; }
            public int CreditCurrencyCode { get; set; }
            public int CreditSerialNumber { get; set; }
            public string CreditTypeDescription { get; set; }
            public Decimal DebtAmount { get; set; }

            public int DetailedAccountTypeCode { get; set; }
            public string ExecutingPartyId { get; set; }
            public string FormattedNextPaymentDate { get; set; }
            public long LoanEndDate { get; set; }
            public Decimal NextPaymentAmount { get; set; }
            public long NextPaymentDate { get; set; }
            public Decimal OriginalLoanPrincipalAmount { get; set; }

            public string PartyCatenatedLoanId { get; set; }
            public string ProductLabel { get; set; }
            public string ProductNickName { get; set; }
            public int UnitedCreditTypeCode { get; set; }
        }
    }
}
