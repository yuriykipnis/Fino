using System;
using System.Collections.Generic;
using GoldMountainShared.Storage.Converters;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace GoldMountainShared.Storage.Documents
{
    public class BankAccount
    {
        [BsonId]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId InternalId { get; set; }

        public Guid Id { get; set; } = Guid.NewGuid();
        public String UserId { get; set; } = String.Empty;
        public Guid ProviderId { get; set; } = Guid.Empty;
        public String ProviderName { get; set; } = string.Empty;
        public String Label { get; set; } = string.Empty;
        public int BankNumber { get; set; } = 0;
        public int BranchNumber { get; set; } = 0;
        public string AccountNumber { get; set; } = string.Empty;
        public Double Balance { get; set; } = Double.NaN;
        
        public IEnumerable<Transaction> Transactions { get; set; } = new List<Transaction>();

        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
