using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldMountainShared.Storage.Documents;
using BankAccount = GoldMountainShared.Storage.Documents.BankAccount;
using RawBankAccount = DataProvider.Providers.Models.BankAccount;
using CreditAccount = GoldMountainShared.Storage.Documents.CreditAccount;
using RawCreditAccount = DataProvider.Providers.Models.CreditAccount;
using Transaction = GoldMountainShared.Storage.Documents.Transaction;

namespace DataProvider.Services
{
    public interface IAccountService
    {
        Task<IEnumerable<BankAccount>> InitiateBankAccountsForProvider(Provider provider);
        Task<IEnumerable<BankAccount>> UpdateBankAccountsForProvider(Provider provider);
        Task<IEnumerable<CreditAccount>> InitiateCreditAccountsForProvider(Provider provider);
        Task<IEnumerable<CreditAccount>> UpdateCreditAccountsForProvider(Provider provider);
    }
}
