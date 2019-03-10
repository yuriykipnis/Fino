using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataProvider.Providers.Interfaces;
using DataProvider.Providers.Models;
using GoldMountainShared.Dto.Shared;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataProvider.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class TransactionController : Controller
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly ICreditCardRepository _creditAccountRepository;
        private readonly IProviderFactory _providerFactory;

        public TransactionController(IProviderRepository providerRepository, IProviderFactory providerFactory,
                                     IBankAccountRepository bankAccountRepository, ICreditCardRepository creditAccountRepository)
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
            IEnumerable<TransactionDoc> transactions = null;

            var bankAccount = await _bankAccountRepository.GetAccount(accountId);
            if (bankAccount != null)
            {
                transactions = await GetTransactionsForBankAccount(accountId, year, month);
            }

            var creditAccount = await _creditAccountRepository.GetCard(accountId);
            if (creditAccount != null)
            {
                transactions = await GetTransactionsForCreditAccount(accountId, year, month);
            }

            var result = AutoMapper.Mapper.Map<IEnumerable<TransactionDto>>(transactions);

            return Ok(result);
        }

        private async Task<IEnumerable<TransactionDoc>> GetTransactionsForBankAccount(String id, int year, int month)
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

        private async Task<IEnumerable<TransactionDoc>> GetTransactionsForCreditAccount(String id, int year, int month)
        {
            // get credentials for this account...
            var account = await _creditAccountRepository.GetCard(id);
            var provider = await _providerRepository.GetProvider(account.ProviderId);
            var dataProvider = await _providerFactory.CreateDataProvider(provider);

            var start = new DateTime(year, month, 1);
            var end = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            var accountDescriptor = AutoMapper.Mapper.Map<CreditCardDescriptor>(account);
            var transactions = (dataProvider as ICreditCardProvider)?
                .GetCardsWithTransactions(new List<CreditCardDescriptor>{accountDescriptor}, start, end);
            dataProvider.Dispose();

            var result = await UpdateNewTransactions(account, null, start);
            return result;
        }

        private async Task<IEnumerable<TransactionDoc>> UpdateNewTransactions(
                            CreditCardDoc account, IEnumerable<BankTransaction> transactions, DateTime date)
        {
            var newTransactions = AutoMapper.Mapper.Map<IEnumerable<TransactionDoc>>(transactions).ToList();

            //account.Transactions.ToList()
            //    .RemoveAll(t => t.PaymentDate.Year.Equals(date.Year) && t.PaymentDate.Month.Equals(date.Month));
            //account.Transactions = newTransactions;

            await _creditAccountRepository.UpdateCard(account.Id, account);

            return newTransactions;
        }

        private async Task<IEnumerable<TransactionDoc>> UpdateNewTransactions(
                    BankAccountDoc account, IEnumerable<BankTransaction> transactions, DateTime date)
        {
            var newTransactions = AutoMapper.Mapper.Map<IEnumerable<TransactionDoc>>(transactions).ToList();

            account.Transactions.ToList()
                .RemoveAll(t => t.PaymentDate.Year.Equals(date.Year) && t.PaymentDate.Month.Equals(date.Month));
            account.Transactions = newTransactions;

            await _bankAccountRepository.UpdateAccount(account.Id, account);

            return newTransactions;
        }
    }
}