using System;
using System.Collections.Generic;
using System.Text;
using GoldMountainShared.Storage.Converters;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace GoldMountainShared.Storage.Documents
{
    public class ContactMessage
    {
        [BsonId]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId InternalId { get; set; }
        public Guid Id { get; set; } = Guid.Empty;

        public String UserId { get; set; } = String.Empty;
        public string Message { get; set; } = string.Empty;
        public Double Email { get; set; } = Double.NaN;
        public Double PhoneNumber { get; set; } = Double.NaN;

        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
