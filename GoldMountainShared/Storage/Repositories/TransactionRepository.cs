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
    public class TransactionRepository : ITransactionRepository
    {
        private readonly DbContext _context = null;

        public TransactionRepository(IOptions<DbSettings> settings)
        {
            _context = new DbContext(settings);
        }


        public async Task<IEnumerable<Transaction>> GetAllTransactions()
        {
            try
            {
                return await _context.Transactions.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<Transaction> GetTransaction(Guid id)
        {
            try
            {
                return await _context.Transactions.Find(t => t.Id.Equals(id)).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<Transaction> GetTransactionByInternalId(string id)
        {
            try
            {
                ObjectId internalId = GetInternalId(id);
                return await _context.Transactions.Find(t => t.InternalId == internalId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddTransaction(Transaction item)
        {
            try
            {
                await _context.Transactions.InsertOneAsync(item);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddTransactions(IEnumerable<Transaction> items)
        {
            try
            {
                await _context.Transactions.InsertManyAsync(items);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveTransaction(Guid id)
        {
            try
            {
                DeleteResult actionResult
                    = await _context.Transactions.DeleteOneAsync(Builders<Transaction>.Filter.Eq("Id", id));

                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdateTransaction(Guid id, Transaction account)
        {
            try
            {
                ReplaceOneResult actionResult = await _context.Transactions.ReplaceOneAsync(n => n.Id.Equals(id),
                    account, new UpdateOptions { IsUpsert = true });
                return actionResult.IsAcknowledged && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveAllTransactions()
        {
            try
            {
                DeleteResult actionResult
                    = await _context.Transactions.DeleteManyAsync(_ => true);

                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<Transaction> FindTransactionByCriteria(Expression<Func<Transaction, bool>> filter)
        {
            try
            {
                return await _context.Transactions.Find(filter).FirstOrDefaultAsync();
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
