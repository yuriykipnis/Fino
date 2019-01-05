using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataProvider.ErrorHandling;
using DataProvider.Providers.Interfaces;
using DataProvider.Providers.Models;
using DataProvider.Services;
using DistributedLock;
using GoldMountainShared.Models;
using GoldMountainShared.Models.Credit;
using GoldMountainShared.Models.Provider;
using GoldMountainShared.Models.Shared;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using CreditAccount = GoldMountainShared.Storage.Documents.CreditAccount;
using RawCreditAccount = DataProvider.Providers.Models.CreditAccount;

namespace DataProvider.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class CreditAccountsController : Controller
    {
        private readonly IProviderRepository _providerRepository;
        private readonly ICreditAccountRepository _accountRepository;
        private readonly IProviderFactory _providerFactory;
        private readonly IAccountService _accountService;
        private readonly IExclusiveLockRepository _exclusiveLockRepository;

        public CreditAccountsController(IProviderRepository providerRepository, IAccountService accountService,
            ICreditAccountRepository accountRepository, IProviderFactory providerFactory,
            IExclusiveLockRepository exclusiveLockRepository)
        {
            _providerRepository = providerRepository;
            _accountRepository = accountRepository;
            _providerFactory = providerFactory;
            _accountService = accountService;
            _exclusiveLockRepository = exclusiveLockRepository;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("CreditAccount")]
        public async Task<IActionResult> Get()
        {
            var accounts = await _accountRepository.GetAllAccounts();
            var result = AutoMapper.Mapper.Map<IEnumerable<CreditAccountDto>>(accounts);
            return Ok(result);
        }

        [HttpGet("users/{userId}/CreditAccounts")]
        public async Task<IActionResult> GetUpdatedAccountsForUser(String userId)
        {
            IActionResult errorResult = null;
            var result = new List<CreditAccount>();
            var providers = await _providerRepository.GetProviders(p => p.UserId.Equals(userId));
            var bankProviders = providers.Where(p => p.Type.Equals(InstitutionType.Credit) && HasExpiredAccount(p));

            var tasks = ParallelProvidersUpdate(bankProviders, result);
            if (tasks.Any(t => !(t.Result is OkResult)))
            {
                errorResult = tasks.FirstOrDefault(t => !(t.Result is OkResult))?.Result;
            }
            return errorResult ?? Ok(result);
        }

        private List<Task<IActionResult>> ParallelProvidersUpdate(IEnumerable<Provider> creditProviders,
            List<CreditAccount> result)
        {
            Barrier barrier = new Barrier(creditProviders.Count() + 1);
            List<Task<IActionResult>> tasks = new List<Task<IActionResult>>();
            //List<Tuple<ExclusiveGlobalLockEngine, String>> lockEngines = new List<Tuple<ExclusiveGlobalLockEngine, String>>();

            foreach (var provider in creditProviders)
            {
                //var uniqueId = Guid.NewGuid().ToString();
                //var lockEngine = new ExclusiveGlobalLockEngine(_exclusiveLockRepository, provider.Id.ToString());
                //lockEngines.Add(new Tuple<ExclusiveGlobalLockEngine, string>(lockEngine, uniqueId));

                //lockEngine.StartCheckingLock(uniqueId, () =>
                //{
                //    var testProvider = _providerRepository.FindProviderByCriteria(p => p.Id == provider.Id).Result;
                //    if (!HasExpiredAccount(testProvider))
                //    {
                //        return;
                //    }

                tasks.Add(Task<IActionResult>.Factory.StartNew(() =>
                {
                    try
                    {
                        var accounts = _accountService.UpdateCreditAccountsForProvider(provider);
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
                
                //    }, (reason) =>
                //    {
                //        Console.WriteLine("Lock Lost, reason: " + reason);
                //    });
                //}
            }

            barrier.SignalAndWait();
            //foreach (var lockEngine in lockEngines)
            //{
            //    lockEngine.Item1.StopCheckingOrReleaseLock(lockEngine.Item2);
            //}
            return tasks;
        }


        //Retrive accounts based on the user credentials. We use Post insted of get in order to pass credintials in the body 
            [HttpPost("CreditAccount")]
        public async Task<IActionResult> Post([FromBody] ProviderCreatingDto providerDto)
        {

            IEnumerable<CreditAccountCreatingDto> result;

            var providerDescriptor = new Provider
            {
                Name = providerDto.Name,
                Type = providerDto.Type,
                Credentials = providerDto.Credentials
            };

            try
            {
                var dataProvider = await _providerFactory.CreateDataProvider(providerDescriptor);
                var accounts = (dataProvider as ICreditAccountProvider)?.GetAccounts();
                dataProvider.Dispose();

                //var accounts = GetFakeAccounts();
                var newAccounts = FilterNewAccount(providerDto, accounts);
                result = AutoMapper.Mapper.Map<IEnumerable<CreditAccountCreatingDto>>(newAccounts);
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

        private IEnumerable<RawCreditAccount> FilterNewAccount(ProviderCreatingDto providerDto, IEnumerable<RawCreditAccount> accounts)
        {
            IEnumerable<RawCreditAccount> newAccounts;
            var p = AutoMapper.Mapper.Map<Provider>(providerDto);
            var provider = _providerRepository.Find(p);
            if (provider != null)
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

        private bool HasExpiredAccount(Provider provider)
        {
            return _accountRepository.GetAccountsByProviderId(provider.Id).Result.ToList()
                .Any(a => a.UpdatedOn.AddDays(1) < DateTime.Now);
        }

        private bool IsAccountExists(RawCreditAccount account)
        {
            var result = _accountRepository.FindAccountByCriteria(a => a.CardNumber.Equals(account.CardNumber) &&
                                                                     a.ExpirationDate.Equals(account.ExpirationDate));
            return result.Result != null;
        }

        private static List<RawCreditAccount> GetFakeAccounts()
        {
            var res = new List<RawCreditAccount>
            {
                new RawCreditAccount {
                    Name = "Amex Business",
                    CardNumber = "2989",
                    ExpirationDate = DateTime.MaxValue
                },

                new RawCreditAccount
                {
                    Name = "Amex Business",
                    CardNumber = "2115",
                    ExpirationDate = DateTime.MaxValue
                },

                new RawCreditAccount
                {
                    Name = "Amex Business",
                    CardNumber = "8058",
                    ExpirationDate = DateTime.MaxValue
                }
            };
            return res;
        }
    }
}