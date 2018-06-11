using System;
using System.Collections.Generic;
using GoldMountainShared.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GoldMountainShared.Storage.Documents
{
    public class Institution
    {
        [BsonId]
        public ObjectId InternalId { get; set; }

        public Guid Id { get; set; } = Guid.NewGuid();
        public String Name { get; set; }
        public IEnumerable<String> Credentials { get; set; }
        public Boolean IsSupported { get; set; }
        public InstitutionType Type { get; set; }

        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
