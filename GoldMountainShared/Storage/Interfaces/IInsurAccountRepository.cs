using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GoldMountainShared.Storage.Documents;

namespace GoldMountainShared.Storage.Interfaces
{
    public interface IInsurAccountRepository
    {
        Task<IEnumerable<SeInsurAccount>> GetAllAccounts();

        Task<SeInsurAccount> GetAccount(Guid id);

        Task<IEnumerable<SeInsurAccount>> GetAccountsByUserId(String userId);

        Task AddAccount(SeInsurAccount item);

        Task AddAccounts(IEnumerable<SeInsurAccount> items);

        Task<bool> RemoveAccount(Guid id);

        Task<bool> UpdateAccount(Guid id, SeInsurAccount account);

        Task<bool> RemoveAllAccounts();

        Task<SeInsurAccount> FindAccountByCriteria(Expression<Func<SeInsurAccount, bool>> filter);
    }
}
