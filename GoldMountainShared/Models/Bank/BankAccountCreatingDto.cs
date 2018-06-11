using System;
using System.Collections.Generic;
using System.Text;

namespace GoldMountainShared.Models.Bank
{
    public class BankAccountCreatingDto
    {
        public string Label { get; set; } = string.Empty;
        public int BankNumber { get; set; } = 0;
        public int BranchNumber { get; set; } = 0;
        public string AccountNumber { get; set; } = string.Empty;
        public Double Balance { get; set; } = Double.NaN;
    }
}
