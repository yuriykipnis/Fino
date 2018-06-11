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
        Task<IEnumerable<PensionFundAccount>> GetAllAccounts();

        Task<PensionFundAccount> GetAccount(Guid id);

        Task<IEnumerable<PensionFundAccount>> GetAccountsByUserId(String userId);

        Task AddAccount(PensionFundAccount item);

        Task AddAccounts(IEnumerable<PensionFundAccount> items);

        Task<bool> RemoveAccount(Guid id);

        Task<bool> UpdateAccount(Guid id, PensionFundAccount account);

        Task<bool> RemoveAllAccounts();

        Task<PensionFundAccount> FindAccountByCriteria(Expression<Func<PensionFundAccount, bool>> filter);
    }
}
