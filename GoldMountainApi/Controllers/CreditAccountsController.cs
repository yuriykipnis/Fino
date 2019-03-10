using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using GoldMountainApi.Controllers.Helper;
using GoldMountainApi.Services;
using GoldMountainShared.Dto.Credit;
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
        private readonly ICreditCardRepository _accountRepository;
        private readonly IDataService _dataService;
        private readonly IValidationHelper _validationHelper;

        public CreditAccountsController(ICreditCardRepository accountRepository, IDataService dataService,
            IValidationHelper validationHelper)
        {
            _accountRepository = accountRepository;
            _dataService = dataService;
            _validationHelper = validationHelper;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("CreditAccounts")]
        public async Task<IEnumerable<CreditCardDto>> Get()
        {
            var accounts = await _accountRepository.GetAllAccounts();
            var result = AutoMapper.Mapper.Map<IEnumerable<CreditCardDto>>(accounts);
            return result;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("CreditAccounts/{id}")]
        public async Task<CreditCardDto> Get(Guid id)
        {
            var account = await _accountRepository.GetAccount(id) ?? new CreditCardDoc();
            var result = AutoMapper.Mapper.Map<CreditCardDto>(account);
            return result;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("user/{userId}/CreditAccounts")]
        public async Task<IEnumerable<CreditCardDto>> GetAccountsForUser(String userId)
        {
            if (!_validationHelper.ValidateUserPermissions(User, userId))
            {
                throw new AuthenticationException();
            }

            var accounts = await _accountRepository.GetAccountsByUserId(userId) ?? new List<CreditCardDoc>();
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

                accounts = await _accountRepository.GetAccountsByUserId(userId) ?? new List<CreditCardDoc>();
            }

            var result = AutoMapper.Mapper.Map<IEnumerable<CreditCardDto>>(accounts);
            return result;
        }

        private async Task<IEnumerable<CreditCardDoc>> UpdateAccounts(string userId, IEnumerable<CreditCardDoc> accounts)
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