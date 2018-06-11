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
    public class MortgageInsurAccountRepository : IMortgageInsurAccountRepository
    {
        private readonly DbContext _context = null;

        public MortgageInsurAccountRepository(IOptions<DbSettings> settings)
        {
            _context = new DbContext(settings);
        }

        public MortgageInsurAccountRepository(DbSettings settings)
        {
            _context = new DbContext(settings);
        }

        public async Task<IEnumerable<MortgageInsurAccount>> GetAllAccounts()
        {
            try
            {
                return await _context.MortgageInsurAccounts.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<MortgageInsurAccount> GetAccount(Guid id)
        {
            try
            {
                return await _context.MortgageInsurAccounts.Find(account => account.Id.Equals(id)).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<IEnumerable<MortgageInsurAccount>> GetAccountsByUserId(String userId)
        {
            try
            {
                return await _context.MortgageInsurAccounts.Find(account => account.UserId.Equals(userId)).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddAccount(MortgageInsurAccount item)
        {
            try
            {
                await _context.MortgageInsurAccounts.InsertOneAsync(item);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddAccounts(IEnumerable<MortgageInsurAccount> items)
        {
            try
            {
                if (!items.Any()) { return; }

                await _context.MortgageInsurAccounts.InsertManyAsync(items);
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
                    = await _context.MortgageInsurAccounts.DeleteOneAsync(Builders<MortgageInsurAccount>.Filter.Eq("Id", id));

                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdateAccount(Guid id, MortgageInsurAccount account)
        {
            try
            {
                ReplaceOneResult actionResult = await _context.MortgageInsurAccounts.ReplaceOneAsync(a => a.Id.Equals(id),
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
                    = await _context.MortgageInsurAccounts.DeleteManyAsync(_ => true);

                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<MortgageInsurAccount> FindAccountByCriteria(Expression<Func<MortgageInsurAccount, bool>> filter)
        {
            try
            {
                return await _context.MortgageInsurAccounts.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }
    }
}
