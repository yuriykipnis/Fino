using System;
using System.Collections.Generic;
using System.Linq;
using DataProvider.Providers.Banks.Hapoalim.Dto;
using DataProvider.Providers.Interfaces;
using DataProvider.Providers.Models;

namespace DataProvider.Providers.Banks.Hapoalim
{
    public class HapoalimAccountProvider : IBankAccountProvider
    {
        private readonly IHapoalimApi _api;

        public HapoalimAccountProvider(IHapoalimApi api)
        {
            _api = api;
        }
        
        public BankAccount GetAccount(BankAccountDescriptor accountDescriptor)
        {
            try
            {
                var accountDto = GenerateAccountByAccountId(accountDescriptor);
                if (accountDto == null)
                {
                    return null;
                }

                var account = GetAccountInfo(accountDto);

                DateTime now = DateTime.Now;
                var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                var transactions = GetAccountTransactions(accountDto, firstDayOfMonth, lastDayOfMonth);
                account.Transactions = transactions;

                return account;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public IEnumerable<BankAccount> GetAccounts()
        {
            IList<BankAccount> result = new List<BankAccount>();
            var accounts = _api.GetAccountsData();
            foreach (var account in accounts)
            {
                result.Add(GetAccountInfo(account));
            }
            return result;
        }

        public IEnumerable<Transaction> GetTransactions(BankAccountDescriptor accountDescriptor, DateTime startTime, DateTime endTime)
        {
            var account = GenerateAccountByAccountId(accountDescriptor);
            return GetAccountTransactions( account, startTime, endTime);
        }

        private HapoalimAccountResponse GenerateAccountByAccountId(BankAccountDescriptor accountDescriptor)
        {
            var accounts = _api.GetAccountsData();
            var account = accounts.FirstOrDefault(a => a.AccountNumber
                .Equals(accountDescriptor?.AccountNumber, StringComparison.CurrentCultureIgnoreCase));

            return account;
        }

        private IList<Transaction> GetAccountTransactions(HapoalimAccountResponse accountDto,  DateTime startTime, DateTime endTime)
        {
            var transactions = _api.GetTransactions(accountDto, startTime, endTime);
            var result = new List<Transaction>();
            foreach (var transaction in transactions.Transactions)
            {
                var eventDate = new DateTime((int) transaction.EventDate / 10000, (int) transaction.EventDate / 100 % 100,
                    (int) transaction.EventDate % 100).AddMinutes((int) transaction.ExpandedEventDate % 100);
                result.Add(new Transaction
                {
                    Id = (long)(transaction.ReferenceNumber + Math.Round(transaction.EventAmount) + Math.Round(transaction.CurrentBalance)),
                    PurchaseDate = eventDate,
                    PaymentDate = eventDate,
                    Description = transaction.ActivityDescription,
                    CurrentBalance = transaction.CurrentBalance,
                    Amount = transaction.EventAmount,
                    Type = transaction.EventActivityTypeCode == 1 ? TransactionType.Income : TransactionType.Expense,
                    SupplierId = transaction.ReferenceNumber.ToString(),
                });
            }

            return result;
        }
       
        private BankAccount GetAccountInfo(HapoalimAccountResponse account)
        {
            var result = new BankAccount
            {
                Label = account.ProductLabel,
                AccountNumber = account.AccountNumber,
                BankNumber = account.BankNumber,
                BranchNumber = account.BranchNumber,
            };

            if (account.AccountClosingReasonCode > 0)
            {
                return result;
            }

            var balance = _api.GetBalance(account);
            result.Balance = balance?.CurrentBalance ?? 0;
            return result;
        }

        public void Dispose()
        {
            _api?.Dispose();
        }
    }
}
