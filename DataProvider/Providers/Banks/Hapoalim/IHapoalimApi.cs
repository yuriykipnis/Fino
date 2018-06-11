using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataProvider.Providers.Banks.Hapoalim.Dto;


namespace DataProvider.Providers.Banks.Hapoalim
{
    public interface IHapoalimApi : IDisposable
    {
        IEnumerable<HapoalimAccountResponse> GetAccountsData();
        HapoalimTransactionsResponse GetTransactions(HapoalimAccountResponse account, DateTime startTime, DateTime endTime);
        HapoalimBalanceResponse GetBalance(HapoalimAccountResponse account);
    }
}
