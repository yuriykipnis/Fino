using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using GoldMountainApi.Controllers.Helper;
using GoldMountainApi.Services;
using GoldMountainShared.Models.Credit;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GoldMountainApi.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    [Authorize]
    public class CreditAccountsController : Controller
    {
        private readonly ICreditAccountRepository _accountRepository;
        private readonly IDataService _dataService;
        private readonly IValidationHelper _validationHelper;

        public CreditAccountsController(ICreditAccountRepository accountRepository, IDataService dataService,
            IValidationHelper validationHelper)
        {
            _accountRepository = accountRepository;
            _dataService = dataService;
            _validationHelper = validationHelper;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("CreditAccounts")]
        public async Task<IEnumerable<CreditAccountDto>> Get()
        {
            var accounts = await _accountRepository.GetAllAccounts();
            var result = AutoMapper.Mapper.Map<IEnumerable<CreditAccountDto>>(accounts);
            return result;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("CreditAccounts/{id}")]
        public async Task<CreditAccountDto> Get(Guid id)
        {
            var account = await _accountRepository.GetAccount(id) ?? new CreditAccount();
            var result = AutoMapper.Mapper.Map<CreditAccountDto>(account);
            return result;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("user/{userId}/CreditAccounts")]
        public async Task<IEnumerable<CreditAccountDto>> GetAccountsForUser(String userId)
        {
            if (!_validationHelper.ValidateUserPermissions(User, userId))
            {
                throw new AuthenticationException();
            }

            var accounts = await _accountRepository.GetAccountsByUserId(userId) ?? new List<CreditAccount>();
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

                accounts = await _accountRepository.GetAccountsByUserId(userId) ?? new List<CreditAccount>();
            }

            var result = AutoMapper.Mapper.Map<IEnumerable<CreditAccountDto>>(accounts);
            return result;
        }

        private async Task<IEnumerable<CreditAccount>> UpdateAccounts(string userId, IEnumerable<CreditAccount> accounts)
        {
            var updatedAccounts = await _dataService.GetCreditAccountsForUserId(userId);
            foreach (var ua in updatedAccounts)
            {
                var accountToUpdate = accounts.FirstOrDefault(a => a.Id.Equals(ua.Id));
                if (accountToUpdate != null)
                {
                    accountToUpdate.Transactions = ua.Transactions;
                    await _accountRepository.UpdateAccount(accountToUpdate.Id, accountToUpdate);
                }
            }

            return updatedAccounts;
        }

        [HttpDelete("CreditAccounts/{id}")]
        public void Delete(Guid id)
        {
            _accountRepository.RemoveAccount(id);
        }
    }
}