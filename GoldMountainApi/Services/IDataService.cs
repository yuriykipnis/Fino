using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoldMountainShared.Dto.Bank;
using GoldMountainShared.Dto.Credit;
using GoldMountainShared.Dto.Shared;
using GoldMountainShared.Storage.Documents;

namespace GoldMountainApi.Services
{
    public interface IDataService
    {
        Task<bool> UpdateAccount(Guid accountId);
        Task<BankAccountDto> GetBankAccount(Guid accountId);
        Task<CreditCardDto> GetCreditAccount(Guid accountId);
        Task<IEnumerable<TransactionDto>> GetTransactionsForAccount(Guid accountId, DateTime month);
        Task<IEnumerable<BankAccountDoc>> GetBankAccountsForUserId(string userId);
        Task<IEnumerable<BankAccountDoc>> GetBankAccounts(IEnumerable<Guid> accounts);
        Task<IEnumerable<CreditCardDoc>> GetCreditAccountsForUserId(string userId);

    }
}
