using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GoldMountainShared.Storage.Documents
{
    public class Transaction
    {
        [BsonId]
        public ObjectId InternalId { get; set; }

        public Guid Id { get; set; } = Guid.NewGuid();
        public TransactionType Type { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.MinValue;
        public DateTime PaymentDate { get; set; } = DateTime.MinValue;
        public String Description { get; set; } = String.Empty;
        public Double Amount { get; set; }
        public Double CurrentBalance { get; set; }

        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
