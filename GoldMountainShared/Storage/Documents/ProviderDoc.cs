using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using GoldMountainShared.Dto;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GoldMountainShared.Storage.Documents
{
    public class ProviderDoc
    {
        public String Id { get; set; } = Guid.NewGuid().ToString();
        public String UserId { get; set; } = String.Empty;
        public String Name { get; set; }
        public InstitutionType Type { get; set; }
        
        public IDictionary<String, String> Credentials { get; set; } = new ConcurrentDictionary<string, string>();
        public IEnumerable<String> Accounts { get; set; } = new List<String>();

        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
