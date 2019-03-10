using System;

namespace GoldMountainShared.Dto.Bank
{
    public class BankAccountCreatingDto
    {
        public string Label { get; set; } = string.Empty;
        public int BankNumber { get; set; } = 0;
        public int BranchNumber { get; set; } = 0;
        public string AccountNumber { get; set; } = string.Empty;
        public Decimal Balance { get; set; } = Decimal.Zero;
    }
}
