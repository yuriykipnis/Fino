using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataProvider.Providers.Interfaces;
using DataProvider.Providers.Models;

namespace DataProvider.Providers.Banks.Tefahot
{
    public class TefahotAccountProvider : IBankAccountProvider
    {
        private readonly ITefahotApi _api;
        private const string ProviderName = "Mizrahi-Tefahot";
        private const int TefahotBankId = 20;

        public TefahotAccountProvider(ITefahotApi api)
        {
            _api = api;
        }

        public IEnumerable<BankAccount> GetAccounts()
        {
            IList<BankAccount> result = new List<BankAccount>();
            var accounts = _api.GetAccounts();

            foreach (var account in accounts)
            {
                result.Add(new BankAccount
                {
                    BankNumber = TefahotBankId,
                    AccountNumber = account.Number,
                    BranchNumber = Convert.ToInt32(account.Branch),
                    Balance = account.Remain,
                    Label = account.Name,
                    Mortgages = new List<Mortgage>(),
                    Transactions = new List<Transaction>()
                });
            }

            return result;
        }


        public BankAccount GetAccount(BankAccountDescriptor accountDescriptor)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Transaction> GetTransactions(BankAccountDescriptor accountDescriptor, DateTime startTime, DateTime endTime)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Mortgage> GetMortgages(BankAccountDescriptor accountDescriptor)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Loan> GetLoans(BankAccountDescriptor accountDescriptor)
        {
            throw new NotImplementedException();
        }

       
        public void Dispose()
        {
            _api?.Dispose();
        }

    }
}
