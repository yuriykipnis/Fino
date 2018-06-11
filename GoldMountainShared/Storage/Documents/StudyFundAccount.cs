using System;
using System.Collections.Generic;
using System.Text;

namespace GoldMountainShared.Storage.Documents
{
    public class StudyFundAccount
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public String UserId { get; set; } = String.Empty;

        public String ProviderName { get; set; } = String.Empty;
        public String EmployerName { get; set; } = String.Empty;
        public String PlanName { get; set; } = String.Empty;
        public String PolicyId { get; set; } = String.Empty;
        public PolicyStatus PolicyStatus { get; set; }
        public Double TotalSavings { get; set; } = 0;
        public Double? DepositFee { get; set; } = 0;
        public Double? SavingFee { get; set; } = 0;
        public Double YearRevenue { get; set; } = 0;
        public double? SaverDeposit { get; set; } = 0;
        public double? EmployerDeposit { get; set; } = 0;
        public DateTime PolicyOpeningDate { get; set; } = DateTime.MinValue;
        public DateTime WithdrawalDate { get; set; } = DateTime.MaxValue;
        public DateTime ValidationDate { get; set; } = DateTime.MinValue;
        
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }

    public enum PolicyStatus
    {
        Active,
        Inactive
    }
}
