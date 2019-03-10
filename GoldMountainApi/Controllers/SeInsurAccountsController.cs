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
    public class SeInsurAccountsController : Controller
    {
        private readonly IInsurAccountRepository _accountRepository;
        private readonly IValidationHelper _validationHelper;

        public SeInsurAccountsController(IInsurAccountRepository accountRepository, IValidationHelper validationHelper)
        {
            _accountRepository = accountRepository;
            _validationHelper = validationHelper;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("SeInsurAccounts")]
        public async Task<IEnumerable<SeInsurAccountDto>> Get()
        {
            var accounts = await _accountRepository.GetAllAccounts();
            var result = AutoMapper.Mapper.Map<IEnumerable<SeInsurAccountDto>>(accounts);
            return result;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("SeInsurAccounts/{id}")]
        public async Task<SeInsurAccountDto> Get(Guid id)
        {
            var account = await _accountRepository.GetAccount(id) ?? new SeInsurAccountDoc();
            var result = AutoMapper.Mapper.Map<SeInsurAccountDto>(account);
            return result;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("user/{userId}/SeInsurAccounts")]
        public async Task<IEnumerable<SeInsurAccountDto>> GetAccountsForUser(String userId)
        {
            var accounts = await _accountRepository.GetAccountsByUserId(userId) ?? new List<SeInsurAccountDoc>();
            if (!accounts.Any())
            {
                var maslekaReader = new Reader();
                maslekaReader.GenerateInsurAccounts(userId);
                accounts = await _accountRepository.GetAccountsByUserId(userId) ?? new List<SeInsurAccountDoc>();
            }

            var result = AutoMapper.Mapper.Map<IEnumerable<SeInsurAccountDto>>(accounts);
            return result;
        }

        [HttpDelete("SeInsurAccounts/{id}")]
        public void Delete(Guid id)
        {
            _accountRepository.RemoveAccount(id);
        }
    }
}