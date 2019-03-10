using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace DataProvider.Providers.Models
{
    public class BankAccount
    {
        public string Id { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public int BankNumber { get; set; } = 0;
        public int BranchNumber { get; set; } = 0;
        public string AccountNumber { get; set; } = string.Empty;
        public Decimal Balance { get; set; } = Decimal.Zero;

        public IList<BankTransaction> Transactions { get; set; } = new List<BankTransaction>();
        public IList<Mortgage> Mortgages { get; set; } = new List<Mortgage>();

        public static string GenerateAccountId(int bankNumber, int branchNumber, string accountNumber)
        {
            return $"{bankNumber}-{branchNumber}-{accountNumber}";
        }
        
    }
}
