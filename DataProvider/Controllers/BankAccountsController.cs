using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DataProvider.ErrorHandling;
using DataProvider.Providers.Interfaces;
using DataProvider.Providers.Models;
using DataProvider.Services;
using GoldMountainShared;
using GoldMountainShared.Models;
using GoldMountainShared.Models.Bank;
using GoldMountainShared.Models.Provider;
using GoldMountainShared.Models.Shared;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using BankAccount = GoldMountainShared.Storage.Documents.BankAccount;
using RawBankAccount = DataProvider.Providers.Models.BankAccount;
using Transaction = GoldMountainShared.Storage.Documents.Transaction;

namespace DataProvider.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class BankAccountsController : Controller
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IBankAccountRepository _accountRepository;
        private readonly IProviderFactory _providerFactory;
        private readonly IAccountService _accountService;

        public BankAccountsController(IProviderRepository providerRepository, 
            IBankAccountRepository accountRepository, 
            IProviderFactory providerFactory,
            IAccountService accountService)
        {
            _providerRepository = providerRepository;
            _accountRepository = accountRepository;
            _providerFactory = providerFactory;
            _accountService = accountService;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("BankAccounts")]
        public async Task<IActionResult> Get()
        {
            var accounts =  await _accountRepository.GetAllAccounts();
            var result = AutoMapper.Mapper.Map<IEnumerable<BankAccountDto>>(accounts);
            return Ok(result);
        }

        [HttpGet("users/{userId}/BankAccounts")]
        public async Task<IActionResult> GetUpdatedAccountsForUser(String userId)
        {
            IActionResult errorResult = null;
            var result = new List<BankAccount>();
            var providers = await _providerRepository.GetProviders(p => p.UserId.Equals(userId));
            var bankProviders = providers.Where(p => p.Type.Equals(InstitutionType.Bank));

            Barrier barrier = new Barrier(bankProviders.Count() + 1);
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
                    finally
                    {
                        barrier.SignalAndWait();
                    }

                    return Ok();
                }));
            }

            barrier.SignalAndWait();
            if (tasks.Any(t => !(t.Result is OkResult)))
            {
                errorResult = tasks.FirstOrDefault(t => !(t.Result is OkResult))?.Result;
            }
            return errorResult ?? Ok(result);
        }
        
        //Retrive accounts based on the user credentials. We use Post insted of get in order to pass credintials in the body 
        [HttpPost("BankAccounts")]
        public async Task<IActionResult> Post([FromBody] ProviderCreatingDto providerDto)
        {
            IEnumerable<BankAccountCreatingDto> result;

            var providerDescriptor = new Provider
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

                var newAccounts = FilterNewAccount(providerDto, accounts);
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

            //var accounts = GetFakeAccounts();

            return Ok(result);
        }

        private IEnumerable<RawBankAccount> FilterNewAccount(ProviderCreatingDto providerDto, IEnumerable<RawBankAccount> accounts)
        {
            IEnumerable<RawBankAccount> newAccounts;
            var p = AutoMapper.Mapper.Map<Provider>(providerDto);
            var provider = _providerRepository.Find(p);
            if (provider?.Result != null)
            {
                newAccounts = (from account in accounts
                    where !IsAccountExists(account, provider.Result.UserId)
                    select account).ToList();
            }
            else
            {
                newAccounts = accounts;
            }
            return newAccounts;
        }

        private bool IsAccountExists(RawBankAccount account, string userId)
        {
            var result = _accountRepository.FindAccountByCriteria(a => 
                a.AccountNumber.Equals(account.AccountNumber) &&
                a.BankNumber.Equals(account.BankNumber) &&
                a.BranchNumber.Equals(account.BranchNumber) &&
                a.UserId.Equals(userId));

            return result.Result != null;
        }

        private static List<RawBankAccount> GetFakeAccounts()
        {
            var res = new List<RawBankAccount>
            {
                new RawBankAccount {
                    AccountNumber = "123432",
                    BankNumber = 12,
                    BranchNumber = 365,
                    Label = "12-365-123432",
                    Balance = 23322
                },

                new RawBankAccount
                {
                    AccountNumber = "569384",
                    BankNumber = 12,
                    BranchNumber = 612,
                    Label = "12-612-569384",
                    Balance = 123
                },
                new RawBankAccount
                {
                    AccountNumber = "343325",
                    BankNumber = 12,
                    BranchNumber = 612,
                    Label = "12-612-343325",
                    Balance = -2334
                }
            };
            return res;
        }
    }
}
