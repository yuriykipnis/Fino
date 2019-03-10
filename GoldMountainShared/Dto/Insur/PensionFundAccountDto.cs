using System;
using GoldMountainShared.Storage.Documents;

namespace GoldMountainShared.Dto.Insur
{
    public class PensionFundAccountDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public String UserId { get; set; } = String.Empty;

        public String ProviderName { get; set; } = String.Empty;
        public String EmployerName { get; set; } = String.Empty;
        public String PlanName { get; set; } = String.Empty;
        public String PolicyId { get; set; } = String.Empty;
        public PolicyStatus PolicyStatus { get; set; }
        public Double TotalSavings { get; set; } = 0;
        public Double ExpectedRetirementSavingsNoPremium { get; set; } = 0;
        public Double MonthlyRetirementPensionNoPremium { get; set; } = 0;
        public Double ExpectedRetirementSavings { get; set; } = 0;
        public Double MonthlyRetirementPension { get; set; } = 0;
        public Double DepositFee { get; set; } = 0;
        public Double SavingFee { get; set; } = 0;
        public Double YearRevenue { get; set; } = 0;
        public Double DeathInsuranceMonthlyAmount { get; set; } = 0;
        public Double DeathInsuranceAmount { get; set; } = 0;

        public DateTime PolicyOpeningDate { get; set; } = DateTime.MinValue;
        public DateTime ValidationDate { get; set; } = DateTime.MinValue;
    }
}
