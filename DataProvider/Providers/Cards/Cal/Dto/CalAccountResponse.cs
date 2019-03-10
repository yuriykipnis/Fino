using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProvider.Providers.Cards.Cal.Dto
{
    public class CalAccountResponse
    {
        public String AccountCurrency { get; set; }
        public String AccountId { get; set; }
        public String AccountNumber { get; set; }
        public Boolean BankAllCardsAddedInd { get; set; }
        public String BankBranchNumber { get; set; }
        public String BankCode { get; set; }
        public String BankCodeDescription { get; set; }
        public String BankForiengName { get; set; }
        public String BankName { get; set; }

        public IEnumerable<CalCardResponse> Cards { get; set; } = new List<CalCardResponse>();
    }
}
