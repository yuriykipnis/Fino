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
    public class LoanRepository : ILoanRepository
    {
        private readonly DbContext _context = null;

        public LoanRepository(IOptions<DbSettings> settings)
        {
            _context = new DbContext(settings);
        }

        public async Task<IEnumerable<Loan>> GetAllLoans()
        {
            try
            {
                return await _context.Loans.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<Loan> GetLoan(Guid id)
        {
            try
            {
                return await _context.Loans.Find(loan => loan.Id.Equals(id)).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddLoan(Loan item)
        {
            try
            {
                await _context.Loans.InsertOneAsync(item);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddLoans(IEnumerable<Loan> items)
        {
            try
            {
                await _context.Loans.InsertManyAsync(items);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<IEnumerable<Loan>> GetLoansByUserId(string id)
        {
            try
            {
                return await _context.Loans.Find(loan => loan.UserId.Equals(id)).ToListAsync();
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
                    = await _context.Loans.DeleteOneAsync(Builders<Loan>.Filter.Eq("Id", id));

                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdateLoan(Guid id, Loan account)
        {
            try
            {
                account.UpdatedOn = DateTime.Now;
                ReplaceOneResult actionResult = await _context.Loans.ReplaceOneAsync(a => a.Id.Equals(id),
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
                    = await _context.Loans.DeleteManyAsync(_ => true);

                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<Loan> FindLoanByCriteria(Expression<Func<Loan, bool>> filter)
        {
            try
            {
                return await _context.Loans.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }
    }
}
