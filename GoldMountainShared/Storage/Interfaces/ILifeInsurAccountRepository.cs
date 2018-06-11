using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GoldMountainShared.Storage.Documents;

namespace GoldMountainShared.Storage.Interfaces
{
    public interface ILifeInsurAccountRepository
    {
        Task<IEnumerable<ProvidentFundAccount>> GetAllAccounts();

        Task<ProvidentFundAccount> GetAccount(Guid id);

        Task<IEnumerable<ProvidentFundAccount>> GetAccountsByUserId(String userId);

        Task AddAccount(ProvidentFundAccount item);

        Task AddAccounts(IEnumerable<ProvidentFundAccount> items);

        Task<bool> RemoveAccount(Guid id);

        Task<bool> UpdateAccount(Guid id, ProvidentFundAccount account);

        Task<bool> RemoveAllAccounts();

        Task<ProvidentFundAccount> FindAccountByCriteria(Expression<Func<ProvidentFundAccount, bool>> filter);
    }
}
