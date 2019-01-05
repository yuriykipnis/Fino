using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GoldMountainShared.Storage.Documents;

namespace GoldMountainShared.Storage.Interfaces
{
    public interface ICreditAccountRepository
    {
        Task<IEnumerable<CreditAccount>> GetAllAccounts();

        Task<CreditAccount> GetAccount(Guid id);

        Task<CreditAccount> GetAccountByInternalId(String id);

        Task<IEnumerable<CreditAccount>> GetAccountsByUserId(String userId);

        Task<IEnumerable<CreditAccount>> GetAccountsByProviderId(Guid providerId);

        Task AddAccount(CreditAccount item);

        Task AddAccounts(IEnumerable<CreditAccount> items);

        Task<bool> RemoveAccount(Guid id);

        Task<bool> UpdateAccount(Guid id, CreditAccount account);

        Task<bool> RemoveAllAccounts();

        Task<CreditAccount> FindAccountByCriteria(Expression<Func<CreditAccount, bool>> filter);
    }
}
