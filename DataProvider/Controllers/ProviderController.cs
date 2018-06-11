using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataProvider.Providers.Interfaces;
using DataProvider.Providers.Models;
using DataProvider.Services;
using GoldMountainShared.Models;
using GoldMountainShared.Models.Credit;
using GoldMountainShared.Models.Bank;
using GoldMountainShared.Models.Provider;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BankAccount = GoldMountainShared.Storage.Documents.BankAccount;
using RawBankAccount = DataProvider.Providers.Models.BankAccount;
using CreditAccount = GoldMountainShared.Storage.Documents.CreditAccount;
using RawCreditAccount = DataProvider.Providers.Models.CreditAccount;
using Transaction = GoldMountainShared.Storage.Documents.Transaction;

namespace DataProvider.Controllers
{
    [Produces("application/json")]
    [Route("api/provider")]
    public class ProviderController : Controller
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly ICreditAccountRepository _creditAccountRepository;
        private readonly IAccountService _accountService;

        public ProviderController(IProviderRepository providerRepository, IProviderFactory providerFactory,
                                  IAccountService accountService,  
                                  IBankAccountRepository bankAccountRepository, 
                                  ICreditAccountRepository creditAccountRepository)
        {
            _providerRepository = providerRepository;
            _bankAccountRepository = bankAccountRepository;
            _creditAccountRepository = creditAccountRepository;
            _accountService = accountService;
        }

        //Get provider with including account refferences for specific user
        [HttpGet("{id}/accounts")]
        public async Task<IActionResult> Get(Guid id)
        {
            //var provider = await _providerRepository.GetProvider(id);
            //var providerWorker = await _providerFactory.CreateDataProvider(provider);
            //var accounts = (providerWorker as IBankAccountProvider)?.GetAccounts();
            //providerWorker.Dispose();
            //var result = AutoMapper.Mapper.Map<IEnumerable<BankAccountCreatingDto>>(accounts);
            
            return BadRequest();
        }
        
        //Add or update provider with new accounts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProviderCreatingDto newProvider)
        {
            if (newProvider == null)
            {
                return BadRequest();
            }

            ProviderDto result;
            try
            {
                result = await AddProvider(newProvider);
            }
            catch (Exception e)
            {
                throw;
            }
            
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
           // _providerRepository.RemoveProvider(id);
            return Ok();
        }

        private async Task<ProviderDto> AddProvider(ProviderCreatingDto newProvider)
        {
            var provider = await GetProvider(newProvider);
            var accountsIds = provider.Accounts?.ToList() ?? new List<Guid>();
            var result = AutoMapper.Mapper.Map<ProviderDto>(provider);

            if (provider.Type == InstitutionType.Bank)
            {
                var accounts = await UpdateBankAccounts(newProvider, provider, accountsIds);
                result.BankAccounts = AutoMapper.Mapper.Map<IEnumerable<BankAccountDto>>(accounts);
            }
            else if (provider.Type == InstitutionType.Credit)
            {
                var creditAccounts = await UpdateCreditAccounts(newProvider, provider, accountsIds);
                result.CreditAccounts = AutoMapper.Mapper.Map<IEnumerable<CreditAccountDto>>(creditAccounts);
            }

            return result;
        }

        private async Task<IEnumerable<BankAccount>> UpdateBankAccounts(ProviderCreatingDto newProvider, Provider provider, List<Guid> accountsIds)
        {
            var bankAccounts = await AddBankAccounts(newProvider, provider.Id);
            accountsIds.AddRange(bankAccounts.Select(a => a.Id).ToList());
            provider.Accounts = accountsIds;
            await _providerRepository.UpdateProvider(provider.Id, provider);

            var accounts = await _accountService.InitiateBankAccountsForProvider(provider);
            foreach (var account in accounts)
            {
                await _bankAccountRepository.UpdateAccount(account.Id, account);
            }
            return accounts;
        }

        private async Task<IEnumerable<CreditAccount>> UpdateCreditAccounts(ProviderCreatingDto newProvider, Provider provider, List<Guid> accountsIds)
        {
            var creditAccounts = await AddCreditAccounts(newProvider, provider.Id);
            accountsIds.AddRange(creditAccounts.Select(a => a.Id).ToList());
            provider.Accounts = accountsIds;
            await _providerRepository.UpdateProvider(provider.Id, provider);

            var accounts = await _accountService.InitiateCreditAccountsForProvider(provider);
            foreach (var account in accounts)
            {
                await _creditAccountRepository.UpdateAccount(account.Id, account);
            }
            return accounts;
        }
        
        private async Task<IEnumerable<BankAccount>> AddBankAccounts(ProviderCreatingDto newProvider, Guid providerId)
        {
            IEnumerable<BankAccount> accounts = new List<BankAccount>();
            if (newProvider.BankAccounts.Any())
            {
                accounts = AutoMapper.Mapper.Map<IEnumerable<BankAccount>>(newProvider.BankAccounts);
                foreach (var bankAccount in accounts)
                {
                    bankAccount.ProviderId = providerId;
                    bankAccount.ProviderName = newProvider.Name;
                    bankAccount.UserId = newProvider.UserId;
                }

                await _bankAccountRepository.AddAccounts(accounts);
            }

            return accounts;
        }

        private async Task<IEnumerable<CreditAccount>> AddCreditAccounts(ProviderCreatingDto newProvider, Guid providerId)
        {
            IEnumerable<CreditAccount> accounts = new List<CreditAccount>();

            if (newProvider.CreditAccounts.Any())
            {
                accounts = AutoMapper.Mapper.Map<IEnumerable<CreditAccount>>(newProvider.CreditAccounts);
                foreach (var creditAccount in accounts)
                {
                    creditAccount.ProviderId = providerId;
                    creditAccount.ProviderName = newProvider.Name;
                    creditAccount.UserId = newProvider.UserId;
                }

                await _creditAccountRepository.AddAccounts(accounts);
            }

            return accounts;
        }

        private async Task<Provider> GetProvider(ProviderCreatingDto newProvider)
        {
            var np = AutoMapper.Mapper.Map<Provider>(newProvider);
            var provider = await _providerRepository.Find(np);
            if (provider == null)
            {
                await _providerRepository.AddProvider(np);
                provider = np;
            }
            return provider;
        }

    }
}