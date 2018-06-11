using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GoldMountainShared.Storage.Repositories
{
    public class EfundAccountRepository : IEfundAccountRepository
    {
        private readonly DbContext _context = null;

        public EfundAccountRepository(IOptions<DbSettings> settings)
        {
            _context = new DbContext(settings);
        }

        public EfundAccountRepository(DbSettings settings)
        {
            _context = new DbContext(settings);
        }

        public async Task<IEnumerable<StudyFundAccount>> GetAllAccounts()
        {
            try
            {
                return await _context.EfundAccounts.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<StudyFundAccount> GetAccount(Guid id)
        {
            try
            {
                return await _context.EfundAccounts.Find(account => account.Id.Equals(id)).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<IEnumerable<StudyFundAccount>> GetAccountsByUserId(String userId)
        {
            try
            {
                return await _context.EfundAccounts.Find(account => account.UserId.Equals(userId)).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddAccount(StudyFundAccount item)
        {
            try
            {
                await _context.EfundAccounts.InsertOneAsync(item);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddAccounts(IEnumerable<StudyFundAccount> items)
        {
            try
            {
                if (!items.Any()) { return; }
                await _context.EfundAccounts.InsertManyAsync(items);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveAccount(Guid id)
        {
            try
            {
                DeleteResult actionResult
                    = await _context.EfundAccounts.DeleteOneAsync(Builders<StudyFundAccount>.Filter.Eq("Id", id));

                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdateAccount(Guid id, StudyFundAccount account)
        {
            try
            {
                ReplaceOneResult actionResult = await _context.EfundAccounts.ReplaceOneAsync(a => a.Id.Equals(id),
                    account, new UpdateOptions { IsUpsert = true });
                return actionResult.IsAcknowledged && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            };
        }

        public async Task<bool> RemoveAllAccounts()
        {
            try
            {
                DeleteResult actionResult
                    = await _context.EfundAccounts.DeleteManyAsync(_ => true);

                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<StudyFundAccount> FindAccountByCriteria(Expression<Func<StudyFundAccount, bool>> filter)
        {
            try
            {
                return await _context.EfundAccounts.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }
    }
}
