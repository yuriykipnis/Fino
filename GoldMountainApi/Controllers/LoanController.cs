using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using GoldMountainApi.Controllers.Helper;
using GoldMountainApi.Services;
using GoldMountainShared.Models.Bank;
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
    public class LoanController : Controller
    {
        //private readonly IBankAccountRepository _accountRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly IDataService _dataService;
        private readonly IValidationHelper _validationHelper;

        public LoanController(ILoanRepository loanRepository, IDataService dataService,
            IValidationHelper validationHelper)
        {
            _loanRepository = loanRepository;
            _dataService = dataService;
            _validationHelper = validationHelper;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("Loans")]
        public async Task<IEnumerable<LoanDto>> Get()
        {
            var accounts = await _loanRepository.GetAllLoans();
            var result = AutoMapper.Mapper.Map<IEnumerable<LoanDto>>(accounts);
            return result;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("Loans/{id}")]
        public async Task<LoanDto> Get(Guid id)
        {
            var loan = await _loanRepository.GetLoan(id) ?? new Loan();
            var result = AutoMapper.Mapper.Map<LoanDto>(loan);
            return result;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("User/{userId}/Loans")]
        public async Task<IEnumerable<LoanDto>> GetUpdatedAccountsForUser(String userId)
        {
            if (!_validationHelper.ValidateUserPermissions(User, userId))
            {
                throw new AuthenticationException();
            }

            var loans = await _loanRepository.GetLoansByUserId(userId) ?? new List<Loan>();
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

                loans = await _loanRepository.GetLoansByUserId(userId) ?? new List<Loan>();
            }

            return AutoMapper.Mapper.Map<IEnumerable<LoanDto>>(loans);
        }
        
        [HttpPut("BankAccounts/{id}/Loans")]
        public void Put(Guid id, [FromBody]double balance)
        {
            //_loanRepository.UpdateLoan(id, 0);
        }

        private async Task<IEnumerable<BankAccount>> UpdateLoans(string userId, IEnumerable<Loan> loans)
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
