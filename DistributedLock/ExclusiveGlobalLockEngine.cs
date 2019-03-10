using System;
using System.Threading;
using System.Threading.Tasks;
using DistributedLock.Interfaces;
using GoldMountainShared.Storage.Interfaces;

namespace DistributedLock
{
    public class ExclusiveGlobalLockEngine : IExclusiveGlobalLockEngine
    {
        private readonly IExclusiveGlobalLock _exclusiveGlobalLock = null;
        private readonly int _lockCheckFrequencyInMilliSeconds = 100;
        private Action _onLockAcquired ;
        private Action<string> _onLockLost;
        private string _clientIdentifier;
        private bool _iAmHoldingTheLock = false;
        private readonly ManualResetEvent _wait = new ManualResetEvent(false);
        private readonly ManualResetEvent _waitEnd = new ManualResetEvent(false); 

        private bool _stopWork;

        public ExclusiveGlobalLockEngine(IExclusiveLockRepository exclusiveLockRepository, String resouceId)
        {
            _exclusiveGlobalLock = new ExclusiveGlobalLock(exclusiveLockRepository, resouceId);
        }
        
        // Start the task of trying to get the lock 
        public void StartCheckingLock(string clientIdentifier, Action onLockAcquired, Action<string> onLockLost)
        {
            if (_onLockAcquired != null)
                throw new Exception("Only one exclusive global lock client is supported by single engine");

            _onLockAcquired = onLockAcquired;
            _onLockLost = onLockLost;
            _clientIdentifier = clientIdentifier;

            TaskFactory tf = new TaskFactory();
            tf.StartNew(DoLockChecking);
        }

        private int GetTimeToWait()
        {
            if(_iAmHoldingTheLock)
            {
                //in case Lock id held by the client - we need to extend the lock only once we get close to the tme of expiration

                var millisecondsTillRenew = (_exclusiveGlobalLock.LockDurationTimeInMilliSeconds ) - _lockCheckFrequencyInMilliSeconds;

                if (millisecondsTillRenew <= 0)
                {
                    millisecondsTillRenew = _lockCheckFrequencyInMilliSeconds;
                }
                var targetTime = _exclusiveGlobalLock.LastAquiredLockTime.Value.AddMilliseconds(millisecondsTillRenew);

                var val=(int)(new TimeSpan(targetTime.Ticks - DateTime.UtcNow.Ticks).TotalMilliseconds);

                if (val < 0)
                    val = 0;

                return val;
            }
            else
            {
                //for lock polling purpose use _lockCheckFrequencyInMilliSeconds value provided at the class constuctor
                return _lockCheckFrequencyInMilliSeconds;
            }

        }

        // The method that is being run by the task for the periodic lock polling or lock maintanance
        private void DoLockChecking()
        {
            int waitTimeout = GetTimeToWait();

            while (!_stopWork) //loop waiting for _stopWork to be flagged as true in order to exit
            {
                if (!_iAmHoldingTheLock)
                {
                   //The lock is not mine - I try to get it 
                    TryGetLock();
                }
                else
                {
                    //I am the holder of the lock - I just want to ensure the lock is mine
                    KeepLock();
                }

                waitTimeout = GetTimeToWait();

                //WAIT for timeout to perform teh check cycle again
                _wait.WaitOne(waitTimeout);

                if (_stopWork)
                {
                    //stop flag is raised - we must exit
                    _waitEnd.Set(); //set the event tha marks end of execution of the task
                    return;
                }

            }
            _waitEnd.Set();//set the event tha marks end of execution of the task
        }

        // Call exclusiveGlobalLockWithMongo class to try to get the lock
        private void TryGetLock()
        {
            var lockAcquired = _exclusiveGlobalLock.TryGetLock(_clientIdentifier);
            if(_iAmHoldingTheLock==false && lockAcquired)
            {
                _iAmHoldingTheLock = true;
                _onLockAcquired();
            }
           
        }
        
        // Call exclusiveGlobalLockWithMongo class ProlongLock method 
        private void KeepLock()
        {
            try
            {
                _exclusiveGlobalLock.ProlongLock(_clientIdentifier);
            }
            catch(Exception ex)
            {
                _iAmHoldingTheLock = false;

                _onLockLost(ex.Message);
            }
        }

        // Method  to stop polling Taks in garcefull way
        public void StopCheckingOrReleaseLock(string clientIdentifier)
        {
            //raise _stopWork flag to stop task executing DoLockChecking method
            _stopWork = true;

            _wait.Set();

            if (_iAmHoldingTheLock) //if we are still holding the lock - release it
                _exclusiveGlobalLock.ReleaseLock(clientIdentifier);
        }
    }
}
