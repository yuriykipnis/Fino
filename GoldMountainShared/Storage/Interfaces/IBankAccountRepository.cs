using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GoldMountainShared.Storage.Documents;

namespace GoldMountainShared.Storage.Interfaces
{
    public interface IBankAccountRepository
    {
        Task<IEnumerable<BankAccount>> GetAllAccounts();

        Task<BankAccount> GetAccount(Guid id);

        Task<BankAccount> GetAccountByInternalId(string id);

        Task<IEnumerable<BankAccount>> GetAccountsByUserId(String id);

        Task AddAccount(BankAccount item);

        Task AddAccounts(IEnumerable<BankAccount> items);

        Task<bool> RemoveAccount(Guid id);

        Task<bool> UpdateAccountBalance(Guid id, Decimal balance);

        Task<bool> UpdateAccount(Guid id, BankAccount account);

        Task<bool> RemoveAllAccounts();

        Task<BankAccount> FindAccountByCriteria(Expression<Func<BankAccount, bool>> filter);
    }
}
