using System;
using System.Collections.Generic;
using GoldMountainShared.Storage.Converters;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace GoldMountainShared.Storage.Documents
{
    public class BankAccountDoc
    {
        [BsonId]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId InternalId { get; set; }

        public String Id { get; set; }
        public String UserId { get; set; }
        public String ProviderId { get; set; }
        public String ProviderName { get; set; } = string.Empty;
        public String Label { get; set; } = string.Empty;
        public int BankNumber { get; set; } = 0;
        public int BranchNumber { get; set; } = 0;
        public string AccountNumber { get; set; } = string.Empty;
        public Decimal Balance { get; set; } = Decimal.Zero;
        
        public IEnumerable<TransactionDoc> Transactions { get; set; } = new List<TransactionDoc>();
        public IEnumerable<MortgageDoc> Mortgages { get; set; } = new List<MortgageDoc>();
        public IEnumerable<LoanDoc> Loans { get; set; } = new List<LoanDoc>();

        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
