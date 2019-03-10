using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataProvider.Providers.Models;
using GoldMountainShared.Storage.Documents;

namespace DataProvider.Services
{
    public interface IAccountService
    {
        Task<IEnumerable<BankAccountDoc>> UpdateBankAccountsForProvider(ProviderDoc provider);
        Task<IEnumerable<CreditCard>> UpdateCreditCards(ProviderDoc provider);
    }
}
