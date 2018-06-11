using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GoldMountainShared.Storage.Documents
{
    public class SeInsurAccount
    {
        [BsonId]
        public ObjectId InternalId { get; set; }

        public Guid Id { get; set; } = Guid.NewGuid();
        public String UserId { get; set; } = String.Empty;

        public String ProviderName { get; set; } = String.Empty;
        public String EmployerName { get; set; } = String.Empty;
        public String PlanName { get; set; } = String.Empty;
        public String PolicyId { get; set; } = String.Empty;
        public PolicyStatus PolicyStatus { get; set; }
        public Double EoyBalance { get; set; } = 0;
        public double? TotalSavings { get; set; } = 0;
        public Double ExpectedRetirementSavingsNoPremium { get; set; } = 0;
        public double? MonthlyRetirementPensionNoPremium { get; set; } = 0;
        public Double ExpectedRetirementSavings { get; set; } = 0;
        public Double MonthlyRetirementPension { get; set; } = 0;
        public double? DepositFee { get; set; } = 0;
        public double? SavingFee { get; set; } = 0;
        public Double YearRevenue { get; set; } = 0;
        public Double DeathInsuranceMonthlyAmount { get; set; } = 0;
        public double? DeathInsuranceAmount { get; set; } = 0;
        public DateTime PolicyOpeningDate { get; set; } = DateTime.MinValue;
        public DateTime ValidationDate { get; set; } = DateTime.MinValue;

        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
