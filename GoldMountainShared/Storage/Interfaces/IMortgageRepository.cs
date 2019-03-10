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
        Task<IEnumerable<MortgageDoc>> GetAllLoans();

        Task<MortgageDoc> GetLoan(Guid id);

        Task AddLoan(MortgageDoc item);

        Task AddLoans(IEnumerable<MortgageDoc> items);

        Task<IEnumerable<MortgageDoc>> GetLoansByUserId(String id);

        Task<bool> RemoveLoan(Guid id);

        Task<bool> UpdateLoan(Guid id, MortgageDoc account);

        Task<bool> RemoveAllLoans();

        Task<MortgageDoc> FindLoanByCriteria(Expression<Func<MortgageDoc, bool>> filter);
    }
}
