using System;
using System.Collections.Generic;
using DataProvider.Providers.Models;

namespace DataProvider.Providers.Interfaces
{
    public interface IBankAccountProvider : IAccountProvider
    {
        BankAccount GetAccount(BankAccountDescriptor accountDescriptor);
        IEnumerable<Transaction> GetTransactions(BankAccountDescriptor accountDescriptor, DateTime startTime, DateTime endTime);
        IEnumerable<Loan> GetLoans(BankAccountDescriptor accountDescriptor);
        IEnumerable<BankAccount> GetAccounts();
    }
}
