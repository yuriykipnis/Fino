using System;
using System.Collections.Generic;

namespace GoldMountainApi.Models
{
    public class OverviewDto
    {
        public Decimal NetWorth;
        public IDictionary<String, Decimal> NetWorthExpenses;
        public IDictionary<String, Decimal> NetWorthIncomes;
        public IDictionary<String, Decimal> CashFlowExpenses;
        public IDictionary<String, Decimal> CashFlowIncomes;
        public IList<InstitutionOverviewDto> ListOfInstitutions;
        public LoanOverviewDto MortgageOverview;
        public LoanOverviewDto LoanOverview;
        public int NumberOfMortgages;
        public int NumberOfLoans;

        public IList<Decimal[]> Loans;
        public IList<Decimal[]> Mortgages;
    }
}
