using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataProvider.Providers.Interfaces;
using DataProvider.Providers.Models;
using DataProvider.Services;
using GoldMountainShared.Dto;
using GoldMountainShared.Dto.Bank;
using GoldMountainShared.Dto.Credit;
using GoldMountainShared.Dto.Provider;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DataProvider.Controllers
{
    [Produces("application/json")]
    [Route("api/provider")]
    public class ProviderController : Controller
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly ICreditCardRepository _creditCardRepository;
        private readonly IAccountService _accountService;

        public ProviderController(IProviderRepository providerRepository, 
                                  IProviderFactory providerFactory,
                                  IAccountService accountService,  
                                  IBankAccountRepository bankAccountRepository, 
                                  ICreditCardRepository creditCardRepository)
        {
            _providerRepository = providerRepository;
            _bankAccountRepository = bankAccountRepository;
            _creditCardRepository = creditCardRepository;
            _accountService = accountService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(String id)
        {
            var providerDoc = await _providerRepository.GetProvider(id);
            if (providerDoc == null)
            {
                return NotFound();
            }

            var result = AutoMapper.Mapper.Map<ProviderDto>(providerDoc);
            return Ok(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProviderCreatingDto newProvider)
        {
            if (newProvider == null)
            {
                return BadRequest();
            }

            ProviderDto result = await AddOrUpdateProvider(newProvider);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(String id)
        {
            var result = await _providerRepository.RemoveProvider(id);
            return Ok(result);
        }

        private async Task<ProviderDto> AddOrUpdateProvider(ProviderCreatingDto newProvider)
        {
            var provider = await FetchOrCreateProvider(newProvider);
            var accountsIds = provider.Accounts?.ToList() ?? new List<String>();
            var result = AutoMapper.Mapper.Map<ProviderDto>(provider);
            
            if (provider.Type == InstitutionType.Bank)
            {
                var accounts = await UpdateBankAccounts(newProvider, provider, accountsIds);
                result.BankAccounts = AutoMapper.Mapper.Map<IEnumerable<BankAccountDto>>(accounts);
            }
            else if (provider.Type == InstitutionType.Credit)
            {
                var creditAccounts = await UpdateCreditCards(newProvider, provider, accountsIds);
                result.CreditCards = AutoMapper.Mapper.Map<IEnumerable<CreditCardDto>>(creditAccounts);
            }

            return result;
        }

        private async Task<IEnumerable<BankAccountDoc>> AddBankAccounts(ProviderCreatingDto newProvider, String providerId)
        {
            IEnumerable<BankAccountDoc> accounts = new List<BankAccountDoc>();
            if (newProvider.BankAccounts.Any())
            {
                accounts = AutoMapper.Mapper.Map<IEnumerable<BankAccountDoc>>(newProvider.BankAccounts);
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

        private async Task<IEnumerable<CreditCardDoc>> AddCreditAccounts(ProviderCreatingDto newProvider, String providerId)
        {
            IEnumerable<CreditCardDoc> accounts = new List<CreditCardDoc>();

            if (newProvider.CreditCards.Any())
            {
                accounts = AutoMapper.Mapper.Map<IEnumerable<CreditCardDoc>>(newProvider.CreditCards);
                foreach (var creditAccount in accounts)
                {
                    creditAccount.ProviderId = providerId;
                    creditAccount.ProviderName = newProvider.Name;
                    creditAccount.UserId = newProvider.UserId;
                }

                await _creditCardRepository.AddCards(accounts);
            }

            return accounts;
        }

        private async Task<IEnumerable<BankAccountDoc>> UpdateBankAccounts(ProviderCreatingDto newProvider, ProviderDoc provider, List<String> accountsIds)
        {
            var bankAccounts = await AddBankAccounts(newProvider, provider.Id);
            accountsIds.AddRange(bankAccounts.Select(a => a.Id).ToList());
            provider.Accounts = accountsIds;
            await _providerRepository.UpdateProvider(provider.Id, provider);

            var accounts = await _accountService.UpdateBankAccountsForProvider(provider);
            foreach (var account in accounts)
            {
                await _bankAccountRepository.UpdateAccount(account.Id, account);
            }
            return accounts;
        }

        private async Task<IEnumerable<CreditCard>> UpdateCreditCards(ProviderCreatingDto newProvider, ProviderDoc provider, List<String> accountsIds)
        {
            var creditCards = await AddCreditAccounts(newProvider, provider.Id);
            accountsIds.AddRange(creditCards.Select(a => a.Id).ToList());
            provider.Accounts = accountsIds;
            await _providerRepository.UpdateProvider(provider.Id, provider);

            var updatedCards = await _accountService.UpdateCreditCards(provider);
            foreach (var updatedCard in updatedCards)
            {

                await _creditCardRepository.UpdateCard(updatedCard.Id,
                    AutoMapper.Mapper.Map<CreditCardDoc>(updatedCard));
            }
            return updatedCards;
        }

        private async Task<ProviderDoc> FetchOrCreateProvider(ProviderCreatingDto newProvider)
        {
            var np = AutoMapper.Mapper.Map<ProviderDoc>(newProvider);
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