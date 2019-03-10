using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GoldMountainShared.Storage.Repositories
{
    public class ExclusiveLockRepository : IExclusiveLockRepository
    {
        private readonly DbContext _context = null;

        public ExclusiveLockRepository(IOptions<DbSettings> settings)
        {
            _context = new DbContext(settings);
        }

        public async Task AddLock(ExclusiveLockStorageDoc item)
        {
            try
            {
                await _context.ExclusiveLock.InsertOneAsync(item);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdateLock(ExclusiveLockStorageDoc item)
        {
            try
            {
                ReplaceOneResult actionResult = await _context.ExclusiveLock
                    .ReplaceOneAsync(l => l.LockId.Equals(item.LockId), item, new UpdateOptions { IsUpsert = true });

                return actionResult.IsAcknowledged && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            };
        }

        public async Task<bool> RemoveLockByCriteria(Expression<Func<ExclusiveLockStorageDoc, bool>> filter)
        {
            try
            {
                var res = _context.ExclusiveLock.DeleteOne(filter);
                return res.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<ExclusiveLockStorageDoc> FindLockByCriteria(Expression<Func<ExclusiveLockStorageDoc, bool>> filter)
        {
            try
            {
                return await _context.ExclusiveLock.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }
    }
}
