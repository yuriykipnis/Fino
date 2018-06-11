using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProvider.Providers.Models
{
    public class CreditAccount
    {
        public String Name { get; set; }
        public String Club { get; set; }
        public String UserName { get; set; }
        public String CardNumber { get; set; }
        public DateTime ExpirationDate { get; set; }
        public String BankAccount { get; set; }
        public String BankName { get; set; }
        public IList<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
