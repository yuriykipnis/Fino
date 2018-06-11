using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataProvider.Providers.Interfaces;
using DataProvider.Providers.Models;
using GoldMountainShared.Models.Bank;
using GoldMountainShared.Models.Shared;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.KeyVault;
using RawTransaction = DataProvider.Providers.Models.Transaction;
using Transaction = GoldMountainShared.Storage.Documents.Transaction;

namespace DataProvider.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class TransactionController : Controller
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly ICreditAccountRepository _creditAccountRepository;
        private readonly IProviderFactory _providerFactory;

        public TransactionController(IProviderRepository providerRepository, IProviderFactory providerFactory,
                                     IBankAccountRepository bankAccountRepository, ICreditAccountRepository creditAccountRepository)
        {
            _providerRepository = providerRepository;
            _bankAccountRepository = bankAccountRepository;
            _creditAccountRepository = creditAccountRepository;
            _providerFactory = providerFactory;
        }

        [HttpGet("accounts/{accountId}/transactions")]
        public async Task<IActionResult> GetTransactionsForAccount(String accountId,
            [FromQuery(Name = "year")] int year, [FromQuery(Name = "month")] int month)
        {
            IEnumerable<Transaction> transactions = null;
            var id = new Guid(accountId);

            var bankAccount = await _bankAccountRepository.GetAccount(id);
            if (bankAccount != null)
            {
                transactions = await GetTransactionsForBankAccount(id, year, month);
            }

            var creditAccount = await _creditAccountRepository.GetAccount(id);
            if (creditAccount != null)
            {
                transactions = await GetTransactionsForCreditAccount(id, year, month);
            }

            var result = AutoMapper.Mapper.Map<IEnumerable<TransactionDto>>(transactions);

            return Ok(result);
        }

        private async Task<IEnumerable<Transaction>> GetTransactionsForBankAccount(Guid id, int year, int month)
        {
            // get credentials for this account...
            var account = await _bankAccountRepository.GetAccount(id);
            var provider = await _providerRepository.GetProvider(account.ProviderId);
            var dataProvider = await _providerFactory.CreateDataProvider(provider);

            var start = new DateTime(year, month, 1);
            var end = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            var accountDescriptor = AutoMapper.Mapper.Map<BankAccountDescriptor>(account);
            var transactions = (dataProvider as IBankAccountProvider)?.GetTransactions(accountDescriptor, start, end);
            dataProvider.Dispose();

            //var accounts = GetFakeTransactions();
            var result = await UpdateNewTransactions(account, transactions, start);
            return result;
        }

        private async Task<IEnumerable<Transaction>> GetTransactionsForCreditAccount(Guid id, int year, int month)
        {
            // get credentials for this account...
            var account = await _creditAccountRepository.GetAccount(id);
            var provider = await _providerRepository.GetProvider(account.ProviderId);
            var dataProvider = await _providerFactory.CreateDataProvider(provider);

            var start = new DateTime(year, month, 1);
            var end = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            var accountDescriptor = AutoMapper.Mapper.Map<CreditAccountDescriptor>(account);
            var transactions = (dataProvider as ICreditAccountProvider)?.GetTransactions(accountDescriptor, start, end);
            dataProvider.Dispose();

            //var accounts = GetFakeTransactions();
            var result = await UpdateNewTransactions(account, transactions, start);
            return result;
        }

        private async Task<IEnumerable<Transaction>> UpdateNewTransactions(
            GoldMountainShared.Storage.Documents.CreditAccount account, IEnumerable<RawTransaction> transactions, DateTime date)
        {
            var newTransactions = AutoMapper.Mapper.Map<IEnumerable<Transaction>>(transactions).ToList();

            account.Transactions.ToList()
                .RemoveAll(t => t.PaymentDate.Year.Equals(date.Year) && t.PaymentDate.Month.Equals(date.Month));
            account.Transactions = newTransactions;

            await _creditAccountRepository.UpdateAccount(account.Id, account);

            return newTransactions;
        }

        private async Task<IEnumerable<Transaction>> UpdateNewTransactions(
            GoldMountainShared.Storage.Documents.BankAccount account, IEnumerable<RawTransaction> transactions, DateTime date)
        {
            var newTransactions = AutoMapper.Mapper.Map<IEnumerable<Transaction>>(transactions).ToList();

            account.Transactions.ToList()
                .RemoveAll(t => t.PaymentDate.Year.Equals(date.Year) && t.PaymentDate.Month.Equals(date.Month));
            account.Transactions = newTransactions;

            await _bankAccountRepository.UpdateAccount(account.Id, account);

            return newTransactions;
        }
    }
}