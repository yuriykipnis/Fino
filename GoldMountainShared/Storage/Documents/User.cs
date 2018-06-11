using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GoldMountainShared.Storage.Documents
{
    public class User
    {
        [BsonId]
        public ObjectId InternalId { get; set; }
        
        public Guid Id { get; set; } = Guid.NewGuid();
        public String Name { get; set; }
        public String Email { get; set; }
        public IEnumerable<Guid> Accounts { get; set; }

        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
