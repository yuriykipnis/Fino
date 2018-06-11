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
    public class LifeInsurAccountRepository: ILifeInsurAccountRepository
    {
        private readonly DbContext _context = null;

        public LifeInsurAccountRepository(IOptions<DbSettings> settings)
        {
            _context = new DbContext(settings);
        }

        public LifeInsurAccountRepository(DbSettings settings)
        {
            _context = new DbContext(settings);
        }

        public async Task<IEnumerable<ProvidentFundAccount>> GetAllAccounts()
        {
            try
            {
                return await _context.LifeInsurAccounts.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<ProvidentFundAccount> GetAccount(Guid id)
        {
            try
            {
                return await _context.LifeInsurAccounts.Find(account => account.Id.Equals(id)).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<IEnumerable<ProvidentFundAccount>> GetAccountsByUserId(String userId)
        {
            try
            {
                return await _context.LifeInsurAccounts.Find(account => account.UserId.Equals(userId)).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddAccount(ProvidentFundAccount item)
        {
            try
            {
                await _context.LifeInsurAccounts.InsertOneAsync(item);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddAccounts(IEnumerable<ProvidentFundAccount> items)
        {
            try
            {
                if (!items.Any()) { return; }

                await _context.LifeInsurAccounts.InsertManyAsync(items);
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
                    = await _context.LifeInsurAccounts.DeleteOneAsync(Builders<ProvidentFundAccount>.Filter.Eq("Id", id));

                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdateAccount(Guid id, ProvidentFundAccount account)
        {
            try
            {
                ReplaceOneResult actionResult = await _context.LifeInsurAccounts.ReplaceOneAsync(a => a.Id.Equals(id),
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
                    = await _context.InsurAccounts.DeleteManyAsync(_ => true);

                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<ProvidentFundAccount> FindAccountByCriteria(Expression<Func<ProvidentFundAccount, bool>> filter)
        {
            try
            {
                return await _context.LifeInsurAccounts.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }
    }
}
