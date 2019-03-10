using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using GoldMountainApi.Controllers.Helper;
using GoldMountainShared.Dto.Insur;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using MaslekaReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoldMountainApi.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    [Authorize]
    public class ProvidentFundAccountsController : Controller
    {
        private readonly ILifeInsurAccountRepository _accountRepository;
        private readonly IValidationHelper _validationHelper;

        public ProvidentFundAccountsController(ILifeInsurAccountRepository accountRepository, IValidationHelper validationHelper)
        {
            _accountRepository = accountRepository;
            _validationHelper = validationHelper;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("ProvidentFundAccounts")]
        public async Task<IEnumerable<ProvidentFundAccountDto>> Get()
        {
            var accounts = await _accountRepository.GetAllAccounts();
            var result = AutoMapper.Mapper.Map<IEnumerable<ProvidentFundAccountDto>>(accounts);
            return result;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("ProvidentFundAccounts/{id}")]
        public async Task<ProvidentFundAccountDto> Get(Guid id)
        {
            var account = await _accountRepository.GetAccount(id) ?? new ProvidentFundAccountDoc();
            var result = AutoMapper.Mapper.Map<ProvidentFundAccountDto>(account);
            return result;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("user/{userId}/ProvidentFundAccounts")]
        public async Task<IEnumerable<ProvidentFundAccountDto>> GetAccountsForUser(String userId)
        {
            var accounts = await _accountRepository.GetAccountsByUserId(userId) ?? new List<ProvidentFundAccountDoc>();
            if (!accounts.Any())
            {
                var maslekaReader = new Reader();
                maslekaReader.GenerateLifeInsurAccounts(userId);
                accounts = await _accountRepository.GetAccountsByUserId(userId) ?? new List<ProvidentFundAccountDoc>();
            }

            var result = AutoMapper.Mapper.Map<IEnumerable<ProvidentFundAccountDto>>(accounts);
            return result;
        }

        [HttpDelete("ProvidentFundAccounts/{id}")]
        public void Delete(Guid id)
        {
            _accountRepository.RemoveAccount(id);
        }
    }
}