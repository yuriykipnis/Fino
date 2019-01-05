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
        Task AddLock(ExclusiveLockStorage item);
        Task<bool> UpdateLock(ExclusiveLockStorage item);
        Task<bool> RemoveLockByCriteria(Expression<Func<ExclusiveLockStorage, bool>> filter);
        Task<ExclusiveLockStorage> FindLockByCriteria(Expression<Func<ExclusiveLockStorage, bool>> filter);
    }
}
