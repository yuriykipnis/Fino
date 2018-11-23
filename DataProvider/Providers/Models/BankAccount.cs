using System;
using System.Collections.Generic;

namespace DataProvider.Providers.Models
{
    public class BankAccount
    {
        public string Label { get; set; } = string.Empty;
        public int BankNumber { get; set; } = 0;
        public int BranchNumber { get; set; } = 0;
        public string AccountNumber { get; set; } = string.Empty;
        public Decimal Balance { get; set; } = Decimal.Zero;

        public IList<Transaction> Transactions { get; set; } = new List<Transaction>();
        public IList<Mortgage> Mortgages { get; set; } = new List<Mortgage>();
    }
}
