using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoldMountainShared.Models.Bank;
using GoldMountainShared.Models.Credit;
using GoldMountainShared.Models.Shared;
using GoldMountainShared.Storage.Documents;

namespace GoldMountainApi.Services
{
    public interface IDataService
    {
        Task<bool> UpdateAccount(Guid accountId);
        Task<BankAccountDto> GetBankAccount(Guid accountId);
        Task<CreditAccountDto> GetCreditAccount(Guid accountId);
        Task<IEnumerable<TransactionDto>> GetTransactionsForAccount(Guid accountId, DateTime month);
        Task<IEnumerable<BankAccount>> GetBankAccountsForUserId(string userId);
        Task<IEnumerable<BankAccount>> GetBankAccounts(IEnumerable<Guid> accounts);
        Task<IEnumerable<CreditAccount>> GetCreditAccountsForUserId(string userId);
    }
}
