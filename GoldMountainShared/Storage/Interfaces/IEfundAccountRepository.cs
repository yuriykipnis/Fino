using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GoldMountainShared.Storage.Documents;

namespace GoldMountainShared.Storage.Interfaces
{
    public interface IEfundAccountRepository
    {
        Task<IEnumerable<StudyFundAccount>> GetAllAccounts();

        Task<StudyFundAccount> GetAccount(Guid id);

        Task<IEnumerable<StudyFundAccount>> GetAccountsByUserId(String userId);

        Task AddAccount(StudyFundAccount item);

        Task AddAccounts(IEnumerable<StudyFundAccount> items);

        Task<bool> RemoveAccount(Guid id);

        Task<bool> UpdateAccount(Guid id, StudyFundAccount account);

        Task<bool> RemoveAllAccounts();

        Task<StudyFundAccount> FindAccountByCriteria(Expression<Func<StudyFundAccount, bool>> filter);
    }
}
