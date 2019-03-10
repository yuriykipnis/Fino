using System;
using System.Collections.Generic;
using DataProvider.Providers.Models;

namespace DataProvider.Providers.Interfaces
{
    public interface IBankAccountProvider : IAccountProvider
    {
        IEnumerable<BankAccount> GetAccounts();
        BankAccount GetAccount(BankAccountDescriptor accountDescriptor);
        IEnumerable<BankAccount> GetAccountsWithAllData(List<CreditCardDescriptor> creditCardDescriptor,
                                                        DateTime startDate, DateTime endDate, 
                                                        bool includeDeatils = false);



        
        IEnumerable<BankTransaction> GetTransactions(BankAccountDescriptor accountDescriptor, DateTime startTime, DateTime endTime);
        IEnumerable<Mortgage> GetMortgages(BankAccountDescriptor accountDescriptor);
        IEnumerable<Loan> GetLoans(BankAccountDescriptor accountDescriptor);
    }
}
