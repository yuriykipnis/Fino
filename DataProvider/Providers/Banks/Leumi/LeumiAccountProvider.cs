using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataProvider.Providers.Banks.Leumi.Dto;
using DataProvider.Providers.Interfaces;
using DataProvider.Providers.Models;

namespace DataProvider.Providers.Banks.Leumi
{
    public class LeumiAccountProvider : IBankAccountProvider
    {
        private readonly ILeumiApi _api;
        private const string ProviderName = "Leumi";
        private const int LeumiBankId = 10;

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
            var result = ConvertToTransactions(transactions);
            return result;
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
            var result = new List<BankAccount>();

            foreach (var account in accounts)
            {
                var newAccount = new BankAccount
                {
                    Label = account.Label,
                    BankNumber = LeumiBankId,
                    BranchNumber = account.BranchNumber,
                    AccountNumber = account.AccountNumber,
                    Balance = account.Balance,

                    Transactions = ConvertToTransactions(account.Transactions),
                    Mortgages = new List<Mortgage>()
                };

                result.Add(newAccount);
            }

            return result;
        }

        private static List<Transaction> ConvertToTransactions(IEnumerable<LeumiTransactionResponse> transactions)
        {
            var result = new List<Transaction>();

            foreach (var transaction in transactions)
            {
                var newTransaction = new Transaction
                {
                    Id = (long)(Convert.ToInt64(transaction.SupplierId) + Math.Round(transaction.Amount) +
                                Math.Round(transaction.CurrentBalance)),
                    Type = transaction.Type,
                    IsFee = false,
                    PurchaseDate = transaction.PurchaseDate,
                    PaymentDate = transaction.PaymentDate,
                    Description = transaction.Description,
                    ProviderName = ProviderName,
                    Amount = transaction.Amount,
                    CurrentBalance = transaction.CurrentBalance,
                    SupplierId = transaction.SupplierId
                };

                result.Add(newTransaction);
            }
            return result;
        }

        public void Dispose()
        {
            _api?.Dispose();
        }
    }
}
