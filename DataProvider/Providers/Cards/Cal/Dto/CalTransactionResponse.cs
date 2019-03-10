using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProvider.Providers.Cards.Cal.Dto
{
    public class CalTransactionsResponse
    {
        public IList<CalTransactionResponse> Transactions { get; set; } = new List<CalTransactionResponse>();
        public CalStatusResponse Response { get; set; }
    }

    public class CalTransactionResponse
    {
        public CalAmount Amount { get; set; }
        public CalAmount DebitAmount { get; set; }
        public String Comments { get; set; }
        public String Currency { get; set; }
        public String CurrentPayment { get; set; }
        public String Date { get; set; }
        public String DebitDate { get; set; }
        public String Id { get; set; }
        public String Notes { get; set; }
        public int Numerator { get; set; }
        public String PayWaveInd { get; set; }
        public Decimal TotalPayments { get; set; }
        public String TransExecutionWay { get; set; }
        public String TransSourceDesk { get; set; }
        public String TransType { get; set; }
        public String TransTypeDesc { get; set; }
    }
}
