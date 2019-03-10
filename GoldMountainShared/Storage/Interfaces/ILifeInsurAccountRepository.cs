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
        Task<IEnumerable<ProvidentFundAccountDoc>> GetAllAccounts();

        Task<ProvidentFundAccountDoc> GetAccount(Guid id);

        Task<IEnumerable<ProvidentFundAccountDoc>> GetAccountsByUserId(String userId);

        Task AddAccount(ProvidentFundAccountDoc item);

        Task AddAccounts(IEnumerable<ProvidentFundAccountDoc> items);

        Task<bool> RemoveAccount(Guid id);

        Task<bool> UpdateAccount(Guid id, ProvidentFundAccountDoc account);

        Task<bool> RemoveAllAccounts();

        Task<ProvidentFundAccountDoc> FindAccountByCriteria(Expression<Func<ProvidentFundAccountDoc, bool>> filter);
    }
}
