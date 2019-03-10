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
    public class CreditCardRepository : ICreditCardRepository
    {
        private readonly DbContext _context = null;

        public CreditCardRepository(IOptions<DbSettings> settings)
        {
            _context = new DbContext(settings);
        }

        public async Task<IEnumerable<CreditCardDoc>> GetAllCards()
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

        public async Task<CreditCardDoc> GetCard(String id)
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

        public async Task<IEnumerable<CreditCardDoc>> GetCardsByUserId(String userId)
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

        public async Task<IEnumerable<CreditCardDoc>> GetCardsByProviderId(String providerId)
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

        public async Task AddCard(CreditCardDoc item)
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

        public async Task AddCards(IEnumerable<CreditCardDoc> items)
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

        public async Task<bool> RemoveCard(String id)
        {
            try
            {
                DeleteResult actionResult
                    = await _context.CreditAccounts.DeleteOneAsync(Builders<CreditCardDoc>.Filter.Eq("Id", id));

                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }
        
        public async Task<bool> UpdateCard(String id, CreditCardDoc account)
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

        public async Task<bool> RemoveAllCards()
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

        public async Task<CreditCardDoc> FindCardByCriteria(Expression<Func<CreditCardDoc, bool>> filter)
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

        private ObjectId GetInternalId(String id)
        {
            if (!ObjectId.TryParse(id, out var internalId))
            {
                internalId = ObjectId.Empty;
            }

            return internalId;
        }
    }
}
