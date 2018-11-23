using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GoldMountainShared.Storage.Documents;

namespace GoldMountainShared.Storage.Interfaces
{
    public interface IMortgageRepository
    {
        Task<IEnumerable<Mortgage>> GetAllLoans();

        Task<Mortgage> GetLoan(Guid id);

        Task AddLoan(Mortgage item);

        Task AddLoans(IEnumerable<Mortgage> items);

        Task<IEnumerable<Mortgage>> GetLoansByUserId(String id);

        Task<bool> RemoveLoan(Guid id);

        Task<bool> UpdateLoan(Guid id, Mortgage account);

        Task<bool> RemoveAllLoans();

        Task<Mortgage> FindLoanByCriteria(Expression<Func<Mortgage, bool>> filter);
    }
}
