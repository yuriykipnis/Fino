using System;
using System.Collections.Generic;
using DataProvider.Providers.Mapping;
using GoldMountainShared.Storage.Converters;
using Newtonsoft.Json;

namespace DataProvider.Providers.Cards.Cal.Dto
{
    public class CalBankDebitsResponse
    {
        public long TotalTransactionsCount { get; set; }
        public IEnumerable<CalBankDebit> Debits { get; set; } = new List<CalBankDebit>();
        public CalStatusResponse Response { get; set; }
    }

    public class CalBankDebit
    {
        public String CardId { get; set; }
        public String CardLast4Digits { get; set; }
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Date { get; set; }
        public long TransactionsCount { get; set; }
        public String Year { get; set; }
        public CalAmount Amount { get; set; }
    }
}
