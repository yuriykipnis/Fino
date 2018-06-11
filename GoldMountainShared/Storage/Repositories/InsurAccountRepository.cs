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
    public class InsurAccountRepository : IInsurAccountRepository
    {
        private readonly DbContext _context = null;

        public InsurAccountRepository(IOptions<DbSettings> settings)
        {
            _context = new DbContext(settings);
        }

        public InsurAccountRepository(DbSettings settings)
        {
            _context = new DbContext(settings);
        }

        public async Task<IEnumerable<SeInsurAccount>> GetAllAccounts()
        {
            try
            {
                return await _context.InsurAccounts.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<SeInsurAccount> GetAccount(Guid id)
        {
            try
            {
                return await _context.InsurAccounts.Find(account => account.Id.Equals(id)).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<IEnumerable<SeInsurAccount>> GetAccountsByUserId(String userId)
        {
            try
            {
                return await _context.InsurAccounts.Find(account => account.UserId.Equals(userId)).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddAccount(SeInsurAccount item)
        {
            try
            {
                await _context.InsurAccounts.InsertOneAsync(item);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddAccounts(IEnumerable<SeInsurAccount> items)
        {
            try
            {
                if (!items.Any()) { return; }

                await _context.InsurAccounts.InsertManyAsync(items);
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
                    = await _context.InsurAccounts.DeleteOneAsync(Builders<SeInsurAccount>.Filter.Eq("Id", id));

                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdateAccount(Guid id, SeInsurAccount account)
        {
            try
            {
                ReplaceOneResult actionResult = await _context.InsurAccounts.ReplaceOneAsync(a => a.Id.Equals(id),
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

        public async Task<SeInsurAccount> FindAccountByCriteria(Expression<Func<SeInsurAccount, bool>> filter)
        {
            try
            {
                return await _context.InsurAccounts.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }
    }
}
