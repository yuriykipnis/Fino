using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GoldMountainShared.Storage.Documents;

namespace GoldMountainShared.Storage.Interfaces
{
    public interface IBankAccountRepository
    {
        Task<IEnumerable<BankAccountDoc>> GetAllAccounts();

        Task<BankAccountDoc> GetAccount(String id);

        Task<BankAccountDoc> GetAccountByInternalId(String id);

        Task<IEnumerable<BankAccountDoc>> GetAccountsByUserId(String id);

        Task<IEnumerable<BankAccountDoc>> GetAccountsByProviderId(String providerId);

        Task AddAccount(BankAccountDoc item);

        Task AddAccounts(IEnumerable<BankAccountDoc> items);

        Task<bool> RemoveAccount(String id);

        Task<bool> UpdateAccountBalance(String id, Decimal balance);

        Task<bool> UpdateAccount(String id, BankAccountDoc account);

        Task<bool> RemoveAllAccounts();

        Task<BankAccountDoc> FindAccountByCriteria(Expression<Func<BankAccountDoc, bool>> filter);
    }
}
