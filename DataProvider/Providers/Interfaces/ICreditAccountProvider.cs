using System;
using System.Collections.Generic;
using DataProvider.Providers.Models;

namespace DataProvider.Providers.Interfaces
{
    public interface ICreditAccountProvider : IAccountProvider
    {
        CreditAccount GetAccount(CreditAccountDescriptor accountDescriptor);
        IEnumerable<Transaction> GetTransactions(CreditAccountDescriptor accountDescriptor, DateTime startTime, DateTime endTime);
        IEnumerable<CreditAccount> GetAccounts();
    }
}
