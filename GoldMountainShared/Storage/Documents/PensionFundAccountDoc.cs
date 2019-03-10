using System;
using System.Collections.Generic;
using System.Text;

namespace GoldMountainShared.Storage.Documents
{
    public class PensionFundAccountDoc
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public String UserId { get; set; } = String.Empty;

        public String ProviderName { get; set; } = String.Empty;
        public String EmployerName { get; set; } = String.Empty;
        public String PlanName { get; set; } = String.Empty;
        public String PolicyId { get; set; } = String.Empty;
        public PolicyStatus PolicyStatus { get; set; }
        public Double TotalSavings { get; set; } = 0;
        public Double? ExpectedRetirementSavingsNoPremium { get; set; } = 0;
        public Double? MonthlyRetirementPensionNoPremium { get; set; } = 0;
        public Double ExpectedRetirementSavings { get; set; } = 0;
        public Double MonthlyRetirementPension { get; set; } = 0;
        public Double? DepositFee { get; set; } = 0;
        public Double? SavingFee { get; set; } = 0;
        public Double YearRevenue { get; set; } = 0;
        public Double SaverDeposit { get; set; } = 0;
        public Double EmployerDeposit { get; set; } = 0;
        public Double PartnerSurvivors { get; set; } = 0;
        public Double ChildrenSurvivors { get; set; } = 0;
        public Double ParentSurvivors { get; set; } = 0;
        public Double InvalidPension { get; set; } = 0;
        public Double WorkDisabilityMonthly { get; set; } = 0;
        public Double WorkDisabilityOneTime { get; set; } = 0;

        public DateTime PolicyOpeningDate { get; set; } = DateTime.MinValue;
        public DateTime ValidationDate { get; set; } = DateTime.MinValue;

    }
}
