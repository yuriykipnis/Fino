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
    public class PensionAccountRepository : IPensionAccountRepository
    {
        private readonly DbContext _context = null;

        public PensionAccountRepository(IOptions<DbSettings> settings)
        {
            _context = new DbContext(settings);
        }

        public PensionAccountRepository(DbSettings settings)
        {
            _context = new DbContext(settings);
        }

        public async Task<IEnumerable<PensionFundAccount>> GetAllAccounts()
        {
            try
            {
                return await _context.PensionAccounts.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<PensionFundAccount> GetAccount(Guid id)
        {
            try
            {
                return await _context.PensionAccounts.Find(account => account.Id.Equals(id)).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<IEnumerable<PensionFundAccount>> GetAccountsByUserId(String userId)
        {
            try
            {
                return await _context.PensionAccounts.Find(account => account.UserId.Equals(userId)).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddAccount(PensionFundAccount item)
        {
            try
            {
                await _context.PensionAccounts.InsertOneAsync(item);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddAccounts(IEnumerable<PensionFundAccount> items)
        {
            try
            {
                if (!items.Any()) { return; }

                await _context.PensionAccounts.InsertManyAsync(items);
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
                    = await _context.PensionAccounts.DeleteOneAsync(Builders<PensionFundAccount>.Filter.Eq("Id", id));

                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdateAccount(Guid id, PensionFundAccount account)
        {
            try
            {
                ReplaceOneResult actionResult = await _context.PensionAccounts.ReplaceOneAsync(a => a.Id.Equals(id),
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
                    = await _context.PensionAccounts.DeleteManyAsync(_ => true);

                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<PensionFundAccount> FindAccountByCriteria(Expression<Func<PensionFundAccount, bool>> filter)
        {
            try
            {
                return await _context.PensionAccounts.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }
    }
}
