using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataProvider.ErrorHandling;
using DataProvider.Providers.Interfaces;
using DataProvider.Services;
using GoldMountainShared.Dto;
using GoldMountainShared.Dto.Bank;
using GoldMountainShared.Dto.Provider;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using DataProvider.Providers.Models;

namespace DataProvider.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class BankAccountController : Controller
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IBankAccountRepository _accountRepository;
        private readonly IProviderFactory _providerFactory;
        private readonly IAccountService _accountService;
        private const int AccountExpirationPeriod = 24;

        public BankAccountController(IAccountService accountService, 
                                      IProviderRepository providerRepository, 
                                      IBankAccountRepository accountRepository, 
                                      IProviderFactory providerFactory)
        {
            _providerRepository = providerRepository;
            _accountRepository = accountRepository;
            _providerFactory = providerFactory;
            _accountService = accountService;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("accounts")]
        public async Task<IActionResult> Get()
        {
            var accounts =  await _accountRepository.GetAllAccounts();
            var result = AutoMapper.Mapper.Map<IEnumerable<BankAccountDto>>(accounts);
            return Ok(result);
        }

        //Retrive accounts based on the user credentials. 
        //We use POST insted of GET in order to pass credintials in the body 
        [HttpPost("accounts")]
        public async Task<IActionResult> GetAccounts([FromBody] ProviderCreatingDto providerDto)
        {
            IEnumerable<BankAccountCreatingDto> result;

            var providerDescriptor = new ProviderDoc
            {
                Name = providerDto.Name,
                Type = providerDto.Type,
                Credentials = providerDto.Credentials
            };

            try
            {
                var dataProvider = await _providerFactory.CreateDataProvider(providerDescriptor);
                var accounts = (dataProvider as IBankAccountProvider)?.GetAccounts();
                dataProvider.Dispose();

                var provider = AutoMapper.Mapper.Map<ProviderDoc>(providerDto);
                var newAccounts = FetchNewAccount(provider, accounts);

                result = AutoMapper.Mapper.Map<IEnumerable<BankAccountCreatingDto>>(newAccounts);
            }
            catch (UnauthorizedAccessException ex)
            {
                return new UnauthorizedActionResult(ex.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Ok(result);
        }

        [HttpGet("users/{userId}/accounts/update")]
        public async Task<IActionResult> UpdateAccounts(String userId)
        {
            IActionResult errorResult = null;
            var result = new List<BankAccountDoc>();
            var providers = await _providerRepository.GetProviders(p => p.UserId.Equals(userId));
            var bankProviders = providers.Where(p => 
                p.Type.Equals(InstitutionType.Bank) 
                && HasOutdatedAccounts(p)).ToList();

            var tasks = UpdateProvidersInParallel(bankProviders, result);
            if (!tasks.Any(t => (t.Result is OkResult)))
            {
                errorResult = tasks.FirstOrDefault(t => !(t.Result is OkResult))?.Result;
            }
            return errorResult ?? Ok(result);
        }

        private List<Task<IActionResult>> UpdateProvidersInParallel(IList<ProviderDoc> bankProviders, List<BankAccountDoc> result)
        {
            List<Task<IActionResult>> tasks = new List<Task<IActionResult>>();

            foreach (var provider in bankProviders)
            {
                tasks.Add(Task<IActionResult>.Factory.StartNew(() =>
                {
                    try
                    {
                        var accounts = _accountService.UpdateBankAccountsForProvider(provider);
                        result.AddRange(accounts.Result);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Log.Error(ex, "UnauthorizedAction in GetUpdatedAccountsForUser - \n");
                        return new UnauthorizedActionResult(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "InternalServerError in GetUpdatedAccountsForUser - \n");
                        return new InternalServerErrorResult(ex.Message);
                    }

                    return Ok();
                }));
            }

            Task.WaitAll(tasks.ToArray());

            return tasks;
        }

        private IEnumerable<BankAccount> FetchNewAccount(ProviderDoc provider, IEnumerable<BankAccount> accounts)
        {
            IEnumerable<BankAccount> newAccounts;
            
            var providerInRepo = _providerRepository.Find(provider);
            if (providerInRepo?.Result != null)
            {
                newAccounts = (from account in accounts
                               where !IsAccountExists(account)
                               select account).ToList();
            }
            else
            {
                newAccounts = accounts;
            }

            return newAccounts;
        }

        private bool IsAccountExists(BankAccount account)
        {
            var result = _accountRepository.FindAccountByCriteria(a => a.Id.Equals(account.Id));
            return result.Result != null;
        }

        private bool HasOutdatedAccounts(ProviderDoc provider)
        {
            return _accountRepository.GetAccountsByProviderId(provider.Id)
                .Result.ToList()
                .Any(a => a.UpdatedOn.AddHours(AccountExpirationPeriod) < DateTime.Now);
        }

        

    }
}
