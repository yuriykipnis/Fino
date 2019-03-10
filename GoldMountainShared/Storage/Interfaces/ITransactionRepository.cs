using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GoldMountainShared.Storage.Documents;

namespace GoldMountainShared.Storage.Interfaces
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<TransactionDoc>> GetAllTransactions();

        Task<TransactionDoc> GetTransaction(Guid id);

        Task<TransactionDoc> GetTransactionByInternalId(string id);

        Task AddTransaction(TransactionDoc item);

        Task AddTransactions(IEnumerable<TransactionDoc> items);

        Task<bool> RemoveTransaction(Guid id);
        
        Task<bool> UpdateTransaction(Guid id, TransactionDoc account);

        Task<bool> RemoveAllTransactions();

        Task<TransactionDoc> FindTransactionByCriteria(Expression<Func<TransactionDoc, bool>> filter);
    }
}
