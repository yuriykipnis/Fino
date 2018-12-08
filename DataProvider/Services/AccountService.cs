using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataProvider.Providers.Interfaces;
using DataProvider.Providers.Models;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using BankAccount = GoldMountainShared.Storage.Documents.BankAccount;
using RawBankAccount = DataProvider.Providers.Models.BankAccount;
using CreditAccount = GoldMountainShared.Storage.Documents.CreditAccount;
using RawCreditAccount = DataProvider.Providers.Models.CreditAccount;
using Transaction = GoldMountainShared.Storage.Documents.Transaction;
using Mortgage = GoldMountainShared.Storage.Documents.Mortgage;
using Loan = GoldMountainShared.Storage.Documents.Loan;

namespace DataProvider.Services
{
    public class AccountService : IAccountService
    {
        private readonly IProviderFactory _providerFactory;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly ICreditAccountRepository _creditAccountRepository;

        public AccountService(IProviderFactory providerFactory, 
            IBankAccountRepository bankAccountRepository, ICreditAccountRepository creditAccountRepository)
        {
            _providerFactory = providerFactory;
            _bankAccountRepository = bankAccountRepository;
            _creditAccountRepository = creditAccountRepository;
        }

        public async Task<IEnumerable<BankAccount>> InitiateBankAccountsForProvider(Provider provider)
        {
            return await GetBankAccountsForProvider(provider, true);
        }

        public async Task<IEnumerable<BankAccount>> UpdateBankAccountsForProvider(Provider provider)
        {
            return await GetBankAccountsForProvider(provider, true);
        }

        public async Task<IEnumerable<CreditAccount>> InitiateCreditAccountsForProvider(Provider provider)
        {
            return await GetCreditAccountsForProvider(provider, true);
        }

        public async Task<IEnumerable<CreditAccount>> UpdateCreditAccountsForProvider(Provider provider)
        {
            return await GetCreditAccountsForProvider(provider, true);
        }

        private async Task<IEnumerable<BankAccount>> GetBankAccountsForProvider(Provider provider, bool isFirstTime)
        {
            var result = new List<BankAccount>();
            using (var dataProvider = await _providerFactory.CreateDataProvider(provider))
            {
                var accounts = ((IBankAccountProvider)dataProvider).GetAccounts();
                foreach (var accountId in provider.Accounts)
                {
                    var account = await UpdateBankAccount(accountId, accounts, dataProvider, isFirstTime);
                    result.Add(account);
                }
            }

            return result;
        }

        private async Task<IEnumerable<CreditAccount>> GetCreditAccountsForProvider(Provider provider, bool isFirstTime)
        {
            var result = new List<CreditAccount>();

            using (var dataProvider = await _providerFactory.CreateDataProvider(provider))
            {
                foreach (var accountId in provider.Accounts)
                {
                    var account = await UpdateCreditAccount(accountId, dataProvider, isFirstTime);
                    result.Add(account);
                }
            }

            return result;
        }

        private async Task<CreditAccount> UpdateCreditAccount(Guid accountId, IAccountProvider dataProvider, bool isFirstTime)
        {
            var accountToUpdate = await _creditAccountRepository.FindAccountByCriteria(a => a.Id.Equals(accountId));
            var accountDescriptor = AutoMapper.Mapper.Map<CreditAccountDescriptor>(accountToUpdate);

            var startTime = isFirstTime ? DateTime.Now.AddYears(-1) : DateTime.Now;
            var endTime = DateTime.Now.AddYears(1);
            var transactions = ((ICreditAccountProvider)dataProvider).GetTransactions(
                accountDescriptor, startTime, endTime);

            accountToUpdate.Transactions = AutoMapper.Mapper.Map<IEnumerable<Transaction>>(transactions);

            return accountToUpdate;
        }

        private async Task<BankAccount> UpdateBankAccount(Guid accountId, IEnumerable<RawBankAccount> accounts, IAccountProvider dataProvider, bool isFirstTime)
        {
            var accountToUpdate = await _bankAccountRepository.FindAccountByCriteria(a => a.Id.Equals(accountId));
            var updatedAccount = RetriveBankAccount(accounts, accountToUpdate);
            var accountDescriptor = AutoMapper.Mapper.Map<BankAccountDescriptor>(accountToUpdate);

            if (updatedAccount == null)
            {
                return accountToUpdate;
            }

            var startOfThisMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var startTime = isFirstTime ? startOfThisMonth.AddYears(-1) : startOfThisMonth;
            var endTime = DateTime.Now;

            var transactions = ((IBankAccountProvider)dataProvider).GetTransactions(accountDescriptor, startTime, endTime);
            var mortgages = ((IBankAccountProvider)dataProvider).GetMortgages(accountDescriptor);
            var loans = ((IBankAccountProvider)dataProvider).GetLoans(accountDescriptor);

            accountToUpdate.Transactions = AutoMapper.Mapper.Map<IEnumerable<Transaction>>(transactions);
            accountToUpdate.Mortgages = AutoMapper.Mapper.Map<IEnumerable<Mortgage>>(mortgages);
            accountToUpdate.Loans = AutoMapper.Mapper.Map<IEnumerable<Loan>>(loans);
            accountToUpdate.Balance = updatedAccount.Balance;

            foreach (var loan in accountToUpdate.Mortgages)
            {
                loan.UserId = accountToUpdate.UserId;
            }

            foreach (var loan in accountToUpdate.Loans)
            {
                loan.UserId = accountToUpdate.UserId;
            }

            return accountToUpdate;
        }

        private static RawBankAccount RetriveBankAccount(IEnumerable<RawBankAccount> accounts, BankAccount accountToUpdate)
        {
            var updatedAccount = accounts.FirstOrDefault(a =>
                a.AccountNumber.Equals(accountToUpdate.AccountNumber)
                && a.BankNumber.Equals(accountToUpdate.BankNumber)
                && a.BranchNumber.Equals(accountToUpdate.BranchNumber));
            return updatedAccount;
        }
    }
}
