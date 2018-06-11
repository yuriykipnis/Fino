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
    public class StudyFundAccountController : Controller
    {
        private readonly IEfundAccountRepository _accountRepository;
        private readonly IValidationHelper _validationHelper;

        public StudyFundAccountController(IEfundAccountRepository accountRepository, IValidationHelper validationHelper)
        {
            _accountRepository = accountRepository;
            _validationHelper = validationHelper;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("StudyFundAccounts")]
        public async Task<IEnumerable<StudyFundAccountDto>> Get()
        {
            var accounts = await _accountRepository.GetAllAccounts();
            var result = AutoMapper.Mapper.Map<IEnumerable<StudyFundAccountDto>>(accounts);
            return result;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("StudyFundAccounts/{id}")]
        public async Task<StudyFundAccountDto> Get(Guid id)
        {
            var account = await _accountRepository.GetAccount(id) ?? new StudyFundAccount();
            var result = AutoMapper.Mapper.Map<StudyFundAccountDto>(account);
            return result;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("user/{userId}/StudyFundAccounts")]
        public async Task<IEnumerable<StudyFundAccountDto>> GetAccountsForUser(String userId)
        {
            var accounts = await _accountRepository.GetAccountsByUserId(userId) ?? new List<StudyFundAccount>();
            if (!accounts.Any())
            {
                var maslekaReader = new Reader();
                maslekaReader.GenerateEfundAccounts(userId);
                accounts = await _accountRepository.GetAccountsByUserId(userId) ?? new List<StudyFundAccount>();
            }
            
            var result = AutoMapper.Mapper.Map<IEnumerable<StudyFundAccountDto>>(accounts);
            return result;
        }
     
        [HttpDelete("StudyFundAccounts/{id}")]
        public void Delete(Guid id)
        {
            _accountRepository.RemoveAccount(id);
        }

        //private IEnumerable<InsurAccount> GetFakeAccount()
        //{
        //    var result = new List<InsurAccount>();

        //    result.Add(new InsurAccount()
        //    {
        //        UserId = Guid.Empty,
        //        ProviderId = Guid.Empty,
        //        Label = string.Empty,
        //        PolicyNumber = string.Empty,
        //        ProductType = string.Empty,
        //        TotalSaving = 0
        //    });
    
        //    return result;

        //}

    }
}