using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using GoldMountainApi.Controllers.Helper;
using GoldMountainShared.Models.Insur;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using MaslekaReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoldMountainApi.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    [Authorize]
    public class PensionFundAccountsController : Controller
    {
        private readonly IPensionAccountRepository _accountRepository;
        private readonly IValidationHelper _validationHelper;

        public PensionFundAccountsController(IPensionAccountRepository accountRepository, IValidationHelper validationHelper)
        {
            _accountRepository = accountRepository;
            _validationHelper = validationHelper;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("PensionFundAccounts")]
        public async Task<IEnumerable<PensionFundAccountDto>> Get()
        {
            var accounts = await _accountRepository.GetAllAccounts();
            var result = AutoMapper.Mapper.Map<IEnumerable<PensionFundAccountDto>>(accounts);
            return result;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("PensionFundAccounts/{id}")]
        public async Task<PensionFundAccountDto> Get(Guid id)
        {
            var account = await _accountRepository.GetAccount(id) ?? new PensionFundAccount();
            var result = AutoMapper.Mapper.Map<PensionFundAccountDto>(account);
            return result;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("user/{userId}/PensionFundAccounts")]
        public async Task<IEnumerable<PensionFundAccountDto>> GetAccountsForUser(String userId)
        {
            var accounts = await _accountRepository.GetAccountsByUserId(userId) ?? new List<PensionFundAccount>();
            if (!accounts.Any())
            {
                var maslekaReader = new Reader();
                maslekaReader.GeneratePensionAccounts(userId);
                accounts = await _accountRepository.GetAccountsByUserId(userId) ?? new List<PensionFundAccount>();
            }

            var result = AutoMapper.Mapper.Map<IEnumerable<PensionFundAccountDto>>(accounts);
            return result;
        }

        [HttpDelete("PensionFundAccounts/{id}")]
        public void Delete(Guid id)
        {
            _accountRepository.RemoveAccount(id);
        }
    }
}