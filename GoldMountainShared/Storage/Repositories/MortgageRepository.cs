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
using DeleteResult = MongoDB.Driver.DeleteResult;
using ReplaceOneResult = MongoDB.Driver.ReplaceOneResult;
using UpdateOptions = MongoDB.Driver.UpdateOptions;

namespace GoldMountainShared.Storage.Repositories
{
    public class MortgageRepository : IMortgageRepository
    {
        private readonly DbContext _context = null;

        public MortgageRepository(IOptions<DbSettings> settings)
        {
            _context = new DbContext(settings);
        }

        public async Task<IEnumerable<MortgageDoc>> GetAllLoans()
        {
            try
            {
                return await _context.Mortgages.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<MortgageDoc> GetLoan(Guid id)
        {
            try
            {
                return await _context.Mortgages.Find(loan => loan.Id.Equals(id)).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddLoan(MortgageDoc item)
        {
            try
            {
                await _context.Mortgages.InsertOneAsync(item);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddLoans(IEnumerable<MortgageDoc> items)
        {
            try
            {
                await _context.Mortgages.InsertManyAsync(items);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<IEnumerable<MortgageDoc>> GetLoansByUserId(string id)
        {
            try
            {
                return await _context.Mortgages.Find(loan => loan.UserId.Equals(id)).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveLoan(Guid id)
        {
            try
            {
                DeleteResult actionResult
                    = await _context.Mortgages.DeleteOneAsync(Builders<MortgageDoc>.Filter.Eq("Id", id));

                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdateLoan(Guid id, MortgageDoc account)
        {
            try
            {
                account.UpdatedOn = DateTime.Now;
                ReplaceOneResult actionResult = await _context.Mortgages.ReplaceOneAsync(a => a.Id.Equals(id),
                    account, new UpdateOptions { IsUpsert = true });
                return actionResult.IsAcknowledged && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveAllLoans()
        {
            try
            {
                DeleteResult actionResult
                    = await _context.Mortgages.DeleteManyAsync(_ => true);

                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<MortgageDoc> FindLoanByCriteria(Expression<Func<MortgageDoc, bool>> filter)
        {
            try
            {
                return await _context.Mortgages.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }
    }
}
