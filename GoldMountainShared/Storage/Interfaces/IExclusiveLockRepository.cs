using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GoldMountainShared.Storage.Documents;

namespace GoldMountainShared.Storage.Interfaces
{
    public interface IExclusiveLockRepository
    {
        Task AddLock(ExclusiveLockStorageDoc item);
        Task<bool> UpdateLock(ExclusiveLockStorageDoc item);
        Task<bool> RemoveLockByCriteria(Expression<Func<ExclusiveLockStorageDoc, bool>> filter);
        Task<ExclusiveLockStorageDoc> FindLockByCriteria(Expression<Func<ExclusiveLockStorageDoc, bool>> filter);
    }
}
