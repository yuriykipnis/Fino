using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using GoldMountainApi.Controllers.Helper;
using GoldMountainApi.Services;
using GoldMountainShared.Dto.Shared;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoldMountainApi.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly ICreditCardRepository _creditAccountRepository;
        private readonly IDataService _dataService;
        private readonly IValidationHelper _validationHelper;

        public TransactionController(IBankAccountRepository bankAccountRepository, ICreditCardRepository creditAccountRepository, 
                                     IDataService dataService, IValidationHelper validationHelper)
        {
            _bankAccountRepository = bankAccountRepository;
            _creditAccountRepository = creditAccountRepository;
            _dataService = dataService;
            _validationHelper = validationHelper;
        }

        [HttpGet("user/{userId}/transactions")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> GetAgregatedTransactionsByDate(String userId,
            [FromQuery(Name = "aggregated")] bool isAggregated,
            [FromQuery(Name = "year")] int year, [FromQuery(Name = "month")] int month)
        {
            IEnumerable<TransactionDto> result;

            if (!_validationHelper.ValidateUserPermissions(User, userId))
            {
                throw new AuthenticationException();
            }

            var transactions = await GetTransactionsForUser(userId, year, month);

            try
            {
                result = AutoMapper.Mapper.Map<IEnumerable<TransactionDto>>(transactions);
            }
            catch (Exception e)
            {
                throw;
            }

            return Ok(result);
        }


        //accounts/{accountId}/transactions?year=2018&month=3
        [HttpGet("accounts/{accountId}/transactions")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> GetTransactionsForAccountByDate(String accountId,
            [FromQuery(Name = "year")] int year, [FromQuery(Name = "month")] int month)
        {
            IEnumerable<TransactionDto> result;

            if (String.IsNullOrEmpty(accountId))
            {
                return BadRequest();
            }

            try
            {
                var id = new Guid(accountId);
                var transactions = await GetTransactionsForAccount(id,year, month);
                result = AutoMapper.Mapper.Map<IEnumerable<TransactionDto>>(transactions);
            }
            catch (Exception e)
            {
                throw;
            }

            return Ok(result);
        }

        [HttpPost("accounts/{accountId}/transactions")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> AddTransactionsToAccount(
            [FromBody] IEnumerable<TransactionDto> transactions, String accountId)
        {
            IEnumerable<TransactionDto> result;
            try
            {
                var id = new Guid(accountId);
                var newTransactions = AutoMapper.Mapper.Map<IEnumerable<TransactionDoc>>(transactions);

                await UpdateAccountWithTransactions(id, newTransactions);
                result = AutoMapper.Mapper.Map<IEnumerable<TransactionDto>>(newTransactions);
            }
            catch (Exception e)
            {
                throw;
            }

            return Ok(result);
        }

        [HttpGet("user/{userId}/banks/fees")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> GetFeesByDate(String userId,
            [FromQuery(Name = "year")] int year, [FromQuery(Name = "month")] int month)
        {
            Decimal result = 0;
            if (!_validationHelper.ValidateUserPermissions(User, userId))
            {
                throw new AuthenticationException();
            }

            var transactions = await GetBankTransactionsForUser(userId, year, month);
            var fees = transactions.Where(t => t.IsFee);

            foreach (var fee in fees)
            {
                result += fee.Amount;
            }

            return Ok(result);
        }


        private async Task<IEnumerable<TransactionDoc>> GetTransactionsForAccount(Guid id, int year, int month)
        {
            var bankAccount = await _bankAccountRepository.GetAccount(id);
            if (bankAccount != null)
            {
                return bankAccount.Transactions.Where(t =>
                {
                    var paymentDate = t.PaymentDate.ToLocalTime();
                    return paymentDate.Year.Equals(year) && paymentDate.Month.Equals(month);
                });
            }

            var creditAccount = await _creditAccountRepository.GetAccount(id);
            if (creditAccount != null)
            {
                return creditAccount.Transactions.Where(t =>
                {
                    var paymentDate = t.PaymentDate.ToLocalTime();
                    return paymentDate.Year.Equals(year) && paymentDate.Month.Equals(month);
                });
            }

            return new List<TransactionDoc>();
        }

        private async Task<IEnumerable<TransactionDoc>> GetBankTransactionsForUser(String userId, int year, int month)
        {
            var banksAccounts = await _bankAccountRepository.GetAccountsByUserId(userId) ?? new List<BankAccountDoc>();

            List<TransactionDoc> transactions = new List<TransactionDoc>();
            foreach (var banksAccount in banksAccounts)
            {
                var trs = GetTransactionsForAccount(banksAccount.Id, year, month).Result;
                transactions.AddRange(trs);
            }

            return transactions;
        }

        private async Task<IEnumerable<TransactionDoc>> GetCreditTransactionsForUser(String userId, int year, int month)
        {
            var creditAccounts = await _creditAccountRepository.GetAccountsByUserId(userId) ?? new List<CreditCardDoc>();

            List<TransactionDoc> transactions = new List<TransactionDoc>();
            foreach (var creditAccount in creditAccounts)
            {
                var trs = GetTransactionsForAccount(creditAccount.Id, year, month).Result;
                transactions.AddRange(trs);
            }

            return transactions;
        }

        private async Task<IEnumerable<TransactionDoc>> GetTransactionsForUser(String userId, int year, int month)
        {
            var bankTransactions = await GetBankTransactionsForUser(userId, year, month);
            var creditTransactions = await GetCreditTransactionsForUser(userId, year, month);

            return bankTransactions.Concat(creditTransactions);
        }

        private async Task<IEnumerable<TransactionDoc>> UpdateAccountWithTransactions(Guid id, IEnumerable<TransactionDoc> transactions)
        {
            var bankAccount = await _bankAccountRepository.GetAccount(id);
            if (bankAccount != null)
            {
                bankAccount.Transactions.ToList().AddRange(transactions);
                await _bankAccountRepository.UpdateAccount(id, bankAccount);
                return bankAccount.Transactions;
            }

            var creditAccount = await _creditAccountRepository.GetAccount(id);
            if (creditAccount != null)
            {
                creditAccount.Transactions.ToList().AddRange(transactions);
                await _creditAccountRepository.UpdateAccount(id, creditAccount);
                return creditAccount.Transactions;
            }

            return new List<TransactionDoc>();
        }
    }
}
