using System;
using System.Collections.Generic;
using DataProvider.Providers.Models;

namespace DataProvider.Providers.Banks.Leumi
{
    public interface ILeumiApi : IDisposable
    {
        IEnumerable<BankAccount> GetAccounts();
        IEnumerable<Transaction> GetTransactions(String accountId, DateTime startTime, DateTime endTime);
        IEnumerable<Mortgage> GetMortgages(String accountId);
        IEnumerable<String> GetBalance(String accountId);
        IEnumerable<Loan> GetLoans(String accountId);
    }
}