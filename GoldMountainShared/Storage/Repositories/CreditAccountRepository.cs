using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GoldMountainShared.Storage.Repositories
{
    public class CreditAccountRepository : ICreditAccountRepository
    {
        private readonly DbContext _context = null;

        public CreditAccountRepository(IOptions<DbSettings> settings)
        {
            _context = new DbContext(settings);
        }

        public async Task<IEnumerable<CreditAccount>> GetAllAccounts()
        {
            try
            {
                return await _context.CreditAccounts.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<CreditAccount> GetAccount(Guid id)
        {
            try
            {
                return await _context.CreditAccounts.Find(account => account.Id.Equals(id)).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<CreditAccount> GetAccountByInternalId(string id)
        {
            try
            {
                ObjectId internalId = GetInternalId(id);
                return await _context.CreditAccounts.Find(account => account.InternalId == internalId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<IEnumerable<CreditAccount>> GetAccountsByUserId(String userId)
        {
            try
            {
                return await _context.CreditAccounts.Find(account => account.UserId.Equals(userId)).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<IEnumerable<CreditAccount>> GetAccountsByProviderId(Guid providerId)
        {
            try
            {
                return await _context.CreditAccounts.Find(account => account.ProviderId.Equals(providerId)).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddAccount(CreditAccount item)
        {
            try
            {
                await _context.CreditAccounts.InsertOneAsync(item);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddAccounts(IEnumerable<CreditAccount> items)
        {
            try
            {
                await _context.CreditAccounts.InsertManyAsync(items);
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
                    = await _context.CreditAccounts.DeleteOneAsync(Builders<CreditAccount>.Filter.Eq("Id", id));

                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }
        
        public async Task<bool> UpdateAccount(Guid id, CreditAccount account)
        {
            try
            {
                account.UpdatedOn = DateTime.Now;
                ReplaceOneResult actionResult = await _context.CreditAccounts.ReplaceOneAsync(a => a.Id.Equals(id),
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
                    = await _context.CreditAccounts.DeleteManyAsync(_ => true);

                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<CreditAccount> FindAccountByCriteria(Expression<Func<CreditAccount, bool>> filter)
        {
            try
            {
                return await _context.CreditAccounts.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        private ObjectId GetInternalId(string id)
        {
            if (!ObjectId.TryParse(id, out var internalId))
            {
                internalId = ObjectId.Empty;
            }

            return internalId;
        }
    }
}
