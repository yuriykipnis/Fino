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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoldMountainApi.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    [Authorize]
    public class MortgageInsurAccountsController : Controller
    {
        private readonly IMortgageInsurAccountRepository _accountRepository;
        private readonly IValidationHelper _validationHelper;

        public MortgageInsurAccountsController(IMortgageInsurAccountRepository accountRepository, IValidationHelper validationHelper)
        {
            _accountRepository = accountRepository;
            _validationHelper = validationHelper;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("MortgageInsurAccounts")]
        public async Task<IEnumerable<MortgageInsurAccountDto>> Get()
        {
            var accounts = await _accountRepository.GetAllAccounts();
            var result = AutoMapper.Mapper.Map<IEnumerable<MortgageInsurAccountDto>>(accounts);
            return result;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("MortgageInsurAccounts/{id}")]
        public async Task<MortgageInsurAccountDto> Get(Guid id)
        {
            var account = await _accountRepository.GetAccount(id) ?? new MortgageInsurAccount();
            var result = AutoMapper.Mapper.Map<MortgageInsurAccountDto>(account);
            return result;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("user/{userId}/MortgageInsurAccounts")]
        public async Task<IEnumerable<MortgageInsurAccountDto>> GetAccountsForUser(String userId)
        {
            var accounts = await _accountRepository.GetAccountsByUserId(userId) ?? new List<MortgageInsurAccount>();
            if (!accounts.Any())
            {
                var maslekaReader = new Reader();
                maslekaReader.GenerateMortgageInsurAccounts(userId);
                accounts = await _accountRepository.GetAccountsByUserId(userId) ?? new List<MortgageInsurAccount>();
            }

            var result = AutoMapper.Mapper.Map<IEnumerable<MortgageInsurAccountDto>>(accounts);
            return result;
        }

        [HttpDelete("MortgageInsurAccounts/{id}")]
        public void Delete(Guid id)
        {
            _accountRepository.RemoveAccount(id);
        }
    }
}