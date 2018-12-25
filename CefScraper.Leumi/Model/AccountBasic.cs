using System;
using System.Collections.Generic;

namespace CefScraper.Leumi.Model
{
    public class AccountBasic
    {
        public int BranchNumber { get; set; }
        public String AccountNumber { get; set; }
        public String Label { get; set; }
        public Decimal Balance { get; set; }
        public IList<LoanBasic> Loans { get; set; }

        public IList<TransactionBasic> Transactions { get; set; }  
        public AccountBasic ()
        {
            Balance = Decimal.Zero;
            Transactions = new List<TransactionBasic>();
            Loans = new List<LoanBasic>();
        }
    }
}
