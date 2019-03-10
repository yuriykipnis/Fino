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
        Task<IEnumerable<SeInsurAccountDoc>> GetAllAccounts();

        Task<SeInsurAccountDoc> GetAccount(Guid id);

        Task<IEnumerable<SeInsurAccountDoc>> GetAccountsByUserId(String userId);

        Task AddAccount(SeInsurAccountDoc item);

        Task AddAccounts(IEnumerable<SeInsurAccountDoc> items);

        Task<bool> RemoveAccount(Guid id);

        Task<bool> UpdateAccount(Guid id, SeInsurAccountDoc account);

        Task<bool> RemoveAllAccounts();

        Task<SeInsurAccountDoc> FindAccountByCriteria(Expression<Func<SeInsurAccountDoc, bool>> filter);
    }
}
