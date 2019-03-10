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
    public class MortgageController : Controller
    {
        //private readonly IBankAccountRepository _accountRepository;
        private readonly IMortgageRepository _mortgageRepository;
        private readonly IDataService _dataService;
        private readonly IValidationHelper _validationHelper;

        public MortgageController(IMortgageRepository mortgageRepository, IDataService dataService,
            IValidationHelper validationHelper)
        {
            _mortgageRepository = mortgageRepository;
            _dataService = dataService;
            _validationHelper = validationHelper;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("mortgages")]
        public async Task<IEnumerable<MortgageDto>> Get()
        {
            var accounts = await _mortgageRepository.GetAllLoans();
            var result = AutoMapper.Mapper.Map<IEnumerable<MortgageDto>>(accounts);
            return result;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("mortgages/{id}")]
        public async Task<MortgageDto> Get(Guid id)
        {
            var loan = await _mortgageRepository.GetLoan(id) ?? new MortgageDoc();
            var result = AutoMapper.Mapper.Map<MortgageDto>(loan);
            return result;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("user/{userId}/mortgages")]
        public async Task<IEnumerable<MortgageDto>> GetUpdatedAccountsForUser(String userId)
        {
            if (!_validationHelper.ValidateUserPermissions(User, userId))
            {
                throw new AuthenticationException();
            }

            var loans = await _mortgageRepository.GetLoansByUserId(userId) ?? new List<MortgageDoc>();
            var needToUpdate = loans.Any(a => DateTime.Compare(a.UpdatedOn.ToLocalTime().AddHours(28), DateTime.Now) < 0);

            if (needToUpdate)
            {
                try
                {
                    await UpdateLoans(userId, loans);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                loans = await _mortgageRepository.GetLoansByUserId(userId) ?? new List<MortgageDoc>();
            }

            return AutoMapper.Mapper.Map<IEnumerable<MortgageDto>>(loans);
        }
        
        [HttpPut("bankAccounts/{id}/mortgages")]
        public void Put(Guid id, [FromBody]Decimal balance)
        {
            //_mortgageRepository.UpdateLoan(id, 0);
        }

        private async Task<IEnumerable<BankAccountDoc>> UpdateLoans(string userId, IEnumerable<MortgageDoc> loans)
        {
            var updatedLoans = await _dataService.GetBankAccountsForUserId(userId);
            foreach (var ua in updatedLoans)
            {
                //var accountToUpdate = accounts.FirstOrDefault(a => a.Id.Equals(ua.Id));
                //if (accountToUpdate != null)
                //{
                //    accountToUpdate.Transactions = ua.Transactions;
                //    accountToUpdate.Balance = ua.Balance;
                //    await _accountRepository.UpdateAccount(accountToUpdate.Id, accountToUpdate);
                //}
            }

            return updatedLoans;
        }
    }
}
