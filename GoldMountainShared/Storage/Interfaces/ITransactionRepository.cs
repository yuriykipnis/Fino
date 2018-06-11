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
        Task<IEnumerable<Transaction>> GetAllTransactions();

        Task<Transaction> GetTransaction(Guid id);

        Task<Transaction> GetTransactionByInternalId(string id);

        Task AddTransaction(Transaction item);

        Task AddTransactions(IEnumerable<Transaction> items);

        Task<bool> RemoveTransaction(Guid id);
        
        Task<bool> UpdateTransaction(Guid id, Transaction account);

        Task<bool> RemoveAllTransactions();

        Task<Transaction> FindTransactionByCriteria(Expression<Func<Transaction, bool>> filter);
    }
}
