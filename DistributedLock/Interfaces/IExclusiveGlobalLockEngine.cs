using System;
using System.Threading.Tasks;

namespace DistibutedLocking.Interfaces
{
    public interface IExclusiveGlobalLockEngine
    {
        /// <summary>
        /// Start periodic attempts to acquire lock 
        /// </summary>
        /// <param name="clientIdentifier">unique identifier of the client request to acquire the lock</param>
        /// <param name="onLockAcquired">callback that allows client to react on acuired lock</param>
        /// <param name="onLockLost">callback for situations where lock is lost</param>
        void StartCheckingLock(string clientIdentifier, Action onLockAcquired, Action<string> onLockLost);

        /// <summary>
        /// Stop the process started in StartCheckingLock method and release lock if is currently held by the client
        /// </summary>
        /// <param name="clientIdentifier"></param>
        void StopCheckingOrReleaseLock(string clientIdentifier);
    }
}
