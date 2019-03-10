using System;
using System.Collections.Generic;
using System.Text;
using GoldMountainShared.Storage.Converters;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace GoldMountainShared.Storage.Documents
{
    public class ContactMessageDoc
    {
        [BsonId]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId InternalId { get; set; }
        public Guid Id { get; set; } = Guid.Empty;

        public String UserId { get; set; } = String.Empty;
        public String Username { get; set; } = String.Empty;
        public String Email { get; set; } = String.Empty;
        public String Phone { get; set; } = String.Empty;
        public String Subject { get; set; } = String.Empty;
        public String Message { get; set; } = String.Empty;

        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
