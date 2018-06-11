using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GoldMountainShared.Storage.Documents;

namespace GoldMountainShared.Storage.Interfaces
{
    public interface IMortgageInsurAccountRepository
    {
        Task<IEnumerable<MortgageInsurAccount>> GetAllAccounts();

        Task<MortgageInsurAccount> GetAccount(Guid id);

        Task<IEnumerable<MortgageInsurAccount>> GetAccountsByUserId(String userId);

        Task AddAccount(MortgageInsurAccount item);

        Task AddAccounts(IEnumerable<MortgageInsurAccount> items);

        Task<bool> RemoveAccount(Guid id);

        Task<bool> UpdateAccount(Guid id, MortgageInsurAccount account);

        Task<bool> RemoveAllAccounts();

        Task<MortgageInsurAccount> FindAccountByCriteria(Expression<Func<MortgageInsurAccount, bool>> filter);
    }
}
