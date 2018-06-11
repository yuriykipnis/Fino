using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProvider.Providers.Models
{
    public class BankAccountDescriptor
    {
        public int BankNumber { get; set; } = 0;
        public int BranchNumber { get; set; } = 0;
        public string AccountNumber { get; set; } = string.Empty;
    }
}
