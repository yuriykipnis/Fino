using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GoldMountainShared.Storage.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly DbContext _context = null;

        public BankAccountRepository(IOptions<DbSettings> settings)
        {
            _context = new DbContext(settings);
        }

       
        public async Task<IEnumerable<BankAccount>> GetAllAccounts()
        {
            try
            {
                return await _context.BankAccounts.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<BankAccount> GetAccount(Guid id)
        {
            try
            {
                return await _context.BankAccounts.Find(account => account.Id.Equals(id)).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<BankAccount> FindAccountByCriteria(Expression<Func<BankAccount, bool>> filter)
        {   
            try
            {
                return await _context.BankAccounts.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<BankAccount> GetAccountByInternalId(string id)
        {
            try
            {
                ObjectId internalId = GetInternalId(id);
                return await _context.BankAccounts.Find(account => account.InternalId == internalId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<IEnumerable<BankAccount>> GetAccountsByUserId(String userId)
        {
            try
            {
                return await _context.BankAccounts.Find(account => account.UserId.Equals(userId)).ToListAsync();
                //return await _context.BankAccounts.Find(Builders<BankAccount>.Filter.Eq("UserId", userId)).ToListAsync();
                
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<IEnumerable<BankAccount>> GetAccountsByProviderId(Guid providerId)
        {
            try
            {
                return await _context.BankAccounts.Find(account => account.ProviderId.Equals(providerId)).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddAccount(BankAccount item)
        {
            try
            {
                await _context.BankAccounts.InsertOneAsync(item);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddAccounts(IEnumerable<BankAccount> items)
        {
            try
            {
                await _context.BankAccounts.InsertManyAsync(items);
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
                    = await _context.BankAccounts.DeleteOneAsync(Builders<BankAccount>.Filter.Eq("Id", id));

                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdateAccountBalance(Guid id, Decimal balance)
        {
            var filter = Builders<BankAccount>.Filter.Eq(s => s.Id, id);
            var update = Builders<BankAccount>.Update
                .Set(s => s.Balance, balance)
                .CurrentDate(s => s.UpdatedOn);

            try
            {
                UpdateResult actionResult = await _context.BankAccounts.UpdateOneAsync(filter, update);
                return actionResult.IsAcknowledged && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdateAccount(Guid id, BankAccount account)
        {
            try
            {
                account.UpdatedOn = DateTime.Now;
                ReplaceOneResult actionResult = await _context.BankAccounts.ReplaceOneAsync(a => a.Id.Equals(id),
                                                account, new UpdateOptions { IsUpsert = true });
                return actionResult.IsAcknowledged && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveAllAccounts()
        {
            try
            {
                DeleteResult actionResult
                    = await _context.BankAccounts.DeleteManyAsync(_ => true);

                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
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
