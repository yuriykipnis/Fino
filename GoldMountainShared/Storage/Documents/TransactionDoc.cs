using System;
using System.Collections.Generic;
using System.Text;
using GoldMountainShared.Storage.Converters;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace GoldMountainShared.Storage.Documents
{
    public class TransactionDoc
    {
        [BsonId]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId InternalId { get; set; }

        public Guid Id { get; set; } = Guid.NewGuid();
        public TransactionType Type { get; set; }
        public Boolean IsFee { get; set; }

        public DateTime PurchaseDate { get; set; } = DateTime.MinValue;
        public DateTime PaymentDate { get; set; } = DateTime.MinValue;
        public String Description { get; set; } = String.Empty;
        public String ProviderName { get; set; } = String.Empty;
        public Decimal Amount { get; set; }
        public Decimal CurrentBalance { get; set; }


        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
