using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistibutedLocking.Interfaces
{
    public interface IExclusiveGlobalLock
    {
        /// <summary>
        /// Try to get exclusive access to the locked resource
        /// </summary>
        /// <param name="clientIdentifier">string that uniquely identifies process making request</param>
        /// <returns>true if lock was acquired</returns>
        bool TryGetLock(string clientIdentifier);

        /// <summary>
        /// Extend the lock the client is currenlty holding
        /// </summary>
        /// <param name="clientIdentifier"></param>
        void ProlongLock(string clientIdentifier);


        /// <summary>
        /// Release lock held by the client
        /// </summary>
        /// <param name="clientIdentifier"></param>
        void ReleaseLock(string clientIdentifier);


        /// <summary>
        /// Returns the last tine when lock was acquired or extended
        /// </summary>
        DateTime? LastAquiredLockTime {get;}

        /// <summary>
        /// Returns lock duration
        /// </summary>
        int LockDurationTimeInMilliSeconds { get; }
    }
}
