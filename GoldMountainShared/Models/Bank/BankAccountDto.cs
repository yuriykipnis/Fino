using System;
using System.Collections.Generic;
using GoldMountainShared.Models.Shared;

namespace GoldMountainShared.Models.Bank
{
    public class BankAccountDto
    {
        public String Id { get; set; } = String.Empty;
        public String ProviderName { get; set; } = string.Empty;
        public String Label { get; set; } = string.Empty;
        public int BankNumber { get; set; } = 0;
        public int BranchNumber { get; set; } = 0;
        public String AccountNumber { get; set; } = string.Empty;
        public Double Balance { get; set; } = Double.NaN;

        public IEnumerable<TransactionDto> Transactions { get; set; } = new List<TransactionDto>();

        public DateTime UpdatedOn { get; set; } = DateTime.MinValue;
    }
}
