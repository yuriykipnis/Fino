using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldMountainApi.Services;
using GoldMountainShared.Models.Shared;
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
        private readonly ICreditAccountRepository _creditAccountRepository;
        private readonly IDataService _dataService;

        public TransactionController(IBankAccountRepository bankAccountRepository, ICreditAccountRepository creditAccountRepository, 
                                     IDataService dataService)
        {
            _bankAccountRepository = bankAccountRepository;
            _creditAccountRepository = creditAccountRepository;
            _dataService = dataService;
        }

        //accounts/{accountId}/transactions?year=2018&month=3
        [HttpGet("accounts/{accountId}/transactions")]
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
        public async Task<IActionResult> AddTransactionsToAccount(
            [FromBody] IEnumerable<TransactionDto> transactions, String accountId)
        {
            IEnumerable<TransactionDto> result;
            try
            {
                var id = new Guid(accountId);
                var newTransactions = AutoMapper.Mapper.Map<IEnumerable<Transaction>>(transactions);

                await UpdateAccountWithTransactions(id, newTransactions);
                result = AutoMapper.Mapper.Map<IEnumerable<TransactionDto>>(newTransactions);
            }
            catch (Exception e)
            {
                throw;
            }

            return Ok(result);
        }

        private async Task<IEnumerable<Transaction>> GetTransactionsForAccount(Guid id, int year, int month)
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

            return new List<Transaction>();
        }

        private async Task<IEnumerable<Transaction>> UpdateAccountWithTransactions(Guid id, IEnumerable<Transaction> transactions)
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

            return new List<Transaction>();
        }
    }
}
