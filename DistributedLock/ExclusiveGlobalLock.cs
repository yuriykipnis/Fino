using System;
using DistributedLock.Interfaces;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;

namespace DistributedLock
{
    public class ExclusiveGlobalLock : IExclusiveGlobalLock
    {
        // unique Id that identifies lock document in mongoDB database
        private readonly String _lockId;
        private DateTime? _lastAquiredLockTime = null;
        public DateTime? LastAquiredLockTime => _lastAquiredLockTime;

        private int _lockDurationTimeInMilliSeconds = 300;
        public int LockDurationTimeInMilliSeconds => _lockDurationTimeInMilliSeconds;

        private readonly IExclusiveLockRepository _distributedLockRepository;

        public ExclusiveGlobalLock(IExclusiveLockRepository distributedLockRepository, String resouceId)
        {
            _distributedLockRepository = distributedLockRepository;
            _lockId = resouceId;
        }
        
        // Attempt by the process to acquire lock
        public bool TryGetLock(string clientIdentifier)
        {
            try
            {
                var foundItem = _distributedLockRepository.FindLockByCriteria(item => item.LockId == _lockId).Result;
                bool isExpired = foundItem !=null && foundItem.LockAcquireTime.AddMilliseconds(_lockDurationTimeInMilliSeconds) < DateTime.UtcNow;
                if (isExpired)
                {
                    //remove expired item from that specific time
                    _distributedLockRepository.RemoveLockByCriteria(item => 
                        item.LockId == _lockId
                        && item.LockAcquireTime == foundItem.LockAcquireTime);
                }

                _lastAquiredLockTime = DateTime.UtcNow;

                //Try to insert lock record into MongoDB - if no error returned -we got the lock
                _distributedLockRepository.AddLock(new ExclusiveLockStorageDoc{
                    LockId = _lockId,
                    LockAcquireTime = _lastAquiredLockTime.Value,
                    LockingProcessId = clientIdentifier
                });
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public void ProlongLock(string clientIdentifier)
        {
           //double check that the process that asks to prolong the process - is the one who holds it
            var exclusiveLockStorageModel = _distributedLockRepository.FindLockByCriteria(item => 
                item.LockId == _lockId).Result;

            if (exclusiveLockStorageModel == null)
            {
                _lastAquiredLockTime = null;
                throw new Exception("Lock does not exist in the database");
            }

            if (exclusiveLockStorageModel.LockingProcessId != clientIdentifier)
            {
                _lastAquiredLockTime = null;
                throw new Exception("Lock is no longer hold by the process asking to prolong it");
            }

            _lastAquiredLockTime = DateTime.UtcNow;
            
            //update lock aquire time
            exclusiveLockStorageModel.LockAcquireTime = _lastAquiredLockTime.Value;

            //update the lock model
            _distributedLockRepository.UpdateLock(exclusiveLockStorageModel);
        }
        
        // Remove the lock from MongoDB if hold by the calling process
        public void ReleaseLock(string clientIdentifier)
        {
            //remove from mongo
            _distributedLockRepository.RemoveLockByCriteria(item => 
                item.LockId == _lockId && item.LockingProcessId == clientIdentifier);

            _lastAquiredLockTime = null;
        }
    }
}
