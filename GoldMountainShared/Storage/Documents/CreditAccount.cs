using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GoldMountainShared.Storage.Documents
{
    public class CreditAccount
    {
        [BsonId]
        public ObjectId InternalId { get; set; }

        public Guid Id { get; set; } = Guid.NewGuid();
        public String UserId { get; set; } = String.Empty;
        public Guid ProviderId { get; set; } = Guid.Empty;

        public String ProviderName { get; set; } = string.Empty;
        public String Name { get; set; } = String.Empty;
        public String Club { get; set; } = String.Empty;
        public String UserName { get; set; } = String.Empty;
        public String CardNumber { get; set; } = String.Empty;
        public DateTime ExpirationDate { get; set; } = DateTime.MaxValue;
        public String BankAccount { get; set; } = String.Empty;
        public String BankName { get; set; } = String.Empty;

        public IEnumerable<Transaction> Transactions { get; set; } = new List<Transaction>();

        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
