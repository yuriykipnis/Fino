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
        Task<IEnumerable<MortgageInsurAccountDoc>> GetAllAccounts();

        Task<MortgageInsurAccountDoc> GetAccount(Guid id);

        Task<IEnumerable<MortgageInsurAccountDoc>> GetAccountsByUserId(String userId);

        Task AddAccount(MortgageInsurAccountDoc item);

        Task AddAccounts(IEnumerable<MortgageInsurAccountDoc> items);

        Task<bool> RemoveAccount(Guid id);

        Task<bool> UpdateAccount(Guid id, MortgageInsurAccountDoc account);

        Task<bool> RemoveAllAccounts();

        Task<MortgageInsurAccountDoc> FindAccountByCriteria(Expression<Func<MortgageInsurAccountDoc, bool>> filter);
    }
}
