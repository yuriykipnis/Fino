using System;
using System.Collections.Generic;
using DataProvider.Providers.Banks.Tefahot.Dto;
using DataProvider.Providers.Models;

namespace DataProvider.Providers.Banks.Tefahot
{
    public interface ITefahotApi : IDisposable
    {
        IEnumerable<TefahotProfileResponse.AccountProfile> GetAccounts();
        IEnumerable<BankTransaction> GetTransactions(String accountId, DateTime startTime, DateTime endTime);
        IEnumerable<TefahotMortgagesResponse.TefahotMortgage> GetMortgages(String accountId);
        IEnumerable<String> GetBalance(String accountId);
        IEnumerable<object> GetLoans(String accountId);
    }
}