using System;
using System.Collections.Generic;
using System.Text;
using GoldMountainShared.Storage.Documents;

namespace GoldMountainShared.Models.Insur
{
    public class MortgageInsurAccountDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public String UserId { get; set; } = String.Empty;

        public String ProviderName { get; set; } = String.Empty;
        public String EmployerName { get; set; } = String.Empty;
        public String PlanName { get; set; } = String.Empty;
        public String PolicyId { get; set; } = String.Empty;
        public PolicyStatus PolicyStatus { get; set; }
        public Decimal DepositFee { get; set; } = 0;
        public Decimal SavingFee { get; set; } = 0;
        public Decimal WorkDisabilityMonthly { get; set; } = 0;
        public Decimal WorkDisabilityOneTime { get; set; } = 0;
        public DateTime PolicyOpeningDate { get; set; } = DateTime.MinValue;
        public DateTime ValidationDate { get; set; } = DateTime.MinValue;
        public List<Coverage> Coverage { get; set; } = new List<Coverage>();
    }
}
