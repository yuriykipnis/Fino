using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using GoldMountainApi.Controllers.Helper;
using GoldMountainApi.Services;
using GoldMountainShared.Models.Bank;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace GoldMountainApi.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    [Authorize]
    public class BankAccountController : Controller
    {
        private readonly IBankAccountRepository _accountRepository;
        private readonly IDataService _dataService;
        private readonly IValidationHelper _validationHelper;

        public BankAccountController(IBankAccountRepository accountRepository, IDataService dataService,
            IValidationHelper validationHelper)
        {
            _accountRepository = accountRepository;
            _dataService = dataService;
            _validationHelper = validationHelper;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("BankAccounts")]
        public async Task<IEnumerable<BankAccountDto>> Get()
        {
            var accounts = await _accountRepository.GetAllAccounts();
            var result = AutoMapper.Mapper.Map<IEnumerable<BankAccountDto>>(accounts);
            return result;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("BankAccounts/{id}")]
        public async Task<BankAccountDto> Get(Guid id)
        {
            var account = await _accountRepository.GetAccount(id) ?? new BankAccount();
            var result = AutoMapper.Mapper.Map<BankAccountDto>(account);
            return result;

            //if (DateTime.Compare(account.UpdatedOn.AddHours(1), DateTime.Now) < 0)
            //{
            //    await _dataService.UpdateAccount(id);
            //    account = await _accountRepository.GetAccount(id) ?? new BankAccount();
            //}
        }
        
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("user/{userId}/BankAccounts")]
        public async Task<IEnumerable<BankAccountDto>> GetUpdatedAccountsForUser(String userId)
        {
            if (!_validationHelper.ValidateUserPermissions(User, userId))
            {
                throw new AuthenticationException();
            }
            
            var accounts = await _accountRepository.GetAccountsByUserId(userId) ?? new List<BankAccount>();
            var needToUpdate = accounts.Any(a => DateTime.Compare(a.UpdatedOn.ToLocalTime().AddHours(28), DateTime.Now) < 0);
            
            if (needToUpdate)
            {
                try
                {
                    await UpdateAccounts(userId, accounts);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                accounts = await _accountRepository.GetAccountsByUserId(userId) ?? new List<BankAccount>();
            }

            return AutoMapper.Mapper.Map<IEnumerable<BankAccountDto>>(accounts);
        }
        
        [HttpPost("BankAccounts")]
        public async Task<IActionResult> Post([FromBody] IEnumerable<BankAccountDto> newAccounts)
        {
            if (newAccounts == null)
            {
                throw new Exception("Something went wrong...");
            }

            try
            {
                foreach (var newAccount in newAccounts)
                {
                    await _accountRepository.AddAccount(new BankAccount
                    {
                        Id = new Guid(newAccount.Id),
                        UserId = newAccount.Id,
                        Label = newAccount.Label,
                        BankNumber = newAccount.BranchNumber,
                        AccountNumber = newAccount.AccountNumber
                    });
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return Ok(newAccounts);
        }

        [HttpPut("BankAccounts/{id}")]
        public void Put(Guid id, [FromBody]Decimal balance)
        {
            _accountRepository.UpdateAccountBalance(id, balance);
        }

        [HttpDelete("BankAccounts/{id}")]
        public void Delete(Guid id)
        {
            _accountRepository.RemoveAccount(id);
        }

        private async Task<IEnumerable<BankAccount>> UpdateAccounts(string userId, IEnumerable<BankAccount> accounts)
        {
            var updatedAccounts = await _dataService.GetBankAccountsForUserId(userId);
            foreach (var ua in updatedAccounts)
            {
                var accountToUpdate = accounts.FirstOrDefault(a => a.Id.Equals(ua.Id));
                if (accountToUpdate != null)
                {
                    accountToUpdate.Transactions = ua.Transactions;
                    accountToUpdate.Mortgages = ua.Mortgages;
                    accountToUpdate.Loans = ua.Loans;
                    accountToUpdate.Balance = ua.Balance;
                    await _accountRepository.UpdateAccount(accountToUpdate.Id, accountToUpdate);
                }
            }
            
            return updatedAccounts;
        }
    }
}