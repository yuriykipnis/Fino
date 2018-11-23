using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataProvider.Providers.Interfaces;
using DataProvider.Providers.Models;

namespace DataProvider.Providers.Banks.Leumi
{
    public class LeumiAccountProvider : IBankAccountProvider
    {
        private readonly ILeumiApi _api;
        private const string _providerName = "Leumi";

        public LeumiAccountProvider(ILeumiApi api)
        {
            _api = api;
        }

        public BankAccount GetAccount(BankAccountDescriptor accountDescriptor)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Transaction> GetTransactions(BankAccountDescriptor accountDescriptor, DateTime startTime, DateTime endTime)
        {
            var transactions = _api.GetTransactions(accountDescriptor.AccountNumber, startTime, endTime);
            return transactions;
        }

        public IEnumerable<Mortgage> GetMortgages(BankAccountDescriptor accountDescriptor)
        {
            return _api.GetMortgages(accountDescriptor.AccountNumber);
        }

        public IEnumerable<Loan> GetLoans(BankAccountDescriptor accountDescriptor)
        {
            return _api.GetLoans(accountDescriptor.AccountNumber);
        }

        public IEnumerable<BankAccount> GetAccounts()
        {
            var accounts = _api.GetAccounts();
            return accounts;
        }


        public void Dispose()
        {
            _api?.Dispose();
        }
    }
}
