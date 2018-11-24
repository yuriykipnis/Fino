using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProvider.Providers.Banks.Leumi.Dto
{
    public class LeumiAccountResponse
    {
        public int BranchNumber { get; set; }
        public String AccountNumber { get; set; }
        public String Label { get; set; }
        public Decimal Balance { get; set; }

        public IList<LeumiTransactionResponse> Transactions { get; set; }
        
        public LeumiAccountResponse()
        {
            Transactions = new List<LeumiTransactionResponse>();
        }
    }
}
