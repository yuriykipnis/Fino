using System;
using System.Collections.Generic;
using System.Text;
using GoldMountainShared.Storage.Documents;

namespace GoldMountainShared.Models.Insur
{
    public class ProvidentFundAccountDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public String UserId { get; set; } = String.Empty;

        public String ProviderName { get; set; } = String.Empty;
        public String PolicyId { get; set; } = String.Empty;
        public PolicyStatus PolicyStatus { get; set; }
        
        public DateTime PolicyOpeningDate { get; set; } = DateTime.MinValue;
        public DateTime ValidationDate { get; set; } = DateTime.MinValue;
    }
}
