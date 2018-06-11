using System;
using System.Collections.Generic;
using GoldMountainShared.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GoldMountainShared.Storage.Documents
{
    public class Provider
    {
        [BsonId]
        public ObjectId InternalId { get; set; }
        
        public Guid Id { get; set; } = Guid.NewGuid();
        public String UserId { get; set; } = String.Empty;
        public String Name { get; set; }
        public InstitutionType Type { get; set; }
        
        public IDictionary<String, String> Credentials { get; set; }
        public IEnumerable<Guid> Accounts { get; set; }

        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
