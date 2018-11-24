using System;
using System.Collections.Generic;
using DataProvider.Providers.Banks.Leumi.Dto;
using DataProvider.Providers.Models;

namespace DataProvider.Providers.Banks.Leumi
{
    public interface ILeumiApi : IDisposable
    {
        IEnumerable<LeumiAccountResponse> GetAccounts();
        IEnumerable<LeumiTransactionResponse> GetTransactions(String accountId, DateTime startTime, DateTime endTime);
        IEnumerable<Mortgage> GetMortgages(String accountId);
        IEnumerable<String> GetBalance(String accountId);
        IEnumerable<Loan> GetLoans(String accountId);
    }
}