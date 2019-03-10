using System;
using MongoDB.Bson.Serialization.Attributes;

namespace GoldMountainShared.Storage.Documents
{
    public class ExclusiveLockStorageDoc
    {
        [BsonId]
        public string LockId { get; set; }

        public string LockingProcessId { get; set; }

        public DateTime LockAcquireTime { get; set; }
    }
}
