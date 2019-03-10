using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GoldMountainShared.Storage.Documents;

namespace GoldMountainShared.Storage.Interfaces
{
    public interface IPensionAccountRepository
    {
        Task<IEnumerable<PensionFundAccountDoc>> GetAllAccounts();

        Task<PensionFundAccountDoc> GetAccount(Guid id);

        Task<IEnumerable<PensionFundAccountDoc>> GetAccountsByUserId(String userId);

        Task AddAccount(PensionFundAccountDoc item);

        Task AddAccounts(IEnumerable<PensionFundAccountDoc> items);

        Task<bool> RemoveAccount(Guid id);

        Task<bool> UpdateAccount(Guid id, PensionFundAccountDoc account);

        Task<bool> RemoveAllAccounts();

        Task<PensionFundAccountDoc> FindAccountByCriteria(Expression<Func<PensionFundAccountDoc, bool>> filter);
    }
}
