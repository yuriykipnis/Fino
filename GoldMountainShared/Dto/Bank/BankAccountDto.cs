using System;
using System.Collections.Generic;
using GoldMountainShared.Dto.Shared;

namespace GoldMountainShared.Dto.Bank
{
    public class BankAccountDto
    {
        public String Id { get; set; } = String.Empty;
        public String ProviderName { get; set; } = string.Empty;
        public String Label { get; set; } = string.Empty;
        public int BankNumber { get; set; } = 0;
        public int BranchNumber { get; set; } = 0;
        public String AccountNumber { get; set; } = string.Empty;
        public Decimal Balance { get; set; } = Decimal.Zero;

        public IEnumerable<TransactionDto> Transactions { get; set; } = new List<TransactionDto>();
        public IEnumerable<MortgageDto> Mortgages { get; set; } = new List<MortgageDto>();
        public IEnumerable<LoanDto> Loans { get; set; } = new List<LoanDto>();

        public DateTime UpdatedOn { get; set; } = DateTime.MinValue;
    }
}
