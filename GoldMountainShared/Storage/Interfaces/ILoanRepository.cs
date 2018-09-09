using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GoldMountainShared.Storage.Documents;

namespace GoldMountainShared.Storage.Interfaces
{
    public interface ILoanRepository
    {
        Task<IEnumerable<Loan>> GetAllLoans();

        Task<Loan> GetLoan(Guid id);

        Task AddLoan(Loan item);

        Task AddLoans(IEnumerable<Loan> items);

        Task<IEnumerable<Loan>> GetLoansByUserId(String id);

        Task<bool> RemoveLoan(Guid id);

        Task<bool> UpdateLoan(Guid id, Loan account);

        Task<bool> RemoveAllLoans();

        Task<Loan> FindLoanByCriteria(Expression<Func<Loan, bool>> filter);
    }
}
