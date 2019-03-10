using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataProvider.ErrorHandling;
using DataProvider.Providers.Interfaces;
using DataProvider.Providers.Models;
using DataProvider.Services;
using GoldMountainShared.Dto;
using GoldMountainShared.Dto.Credit;
using GoldMountainShared.Dto.Provider;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace DataProvider.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class CreditCardController : Controller
    {
        private readonly IProviderRepository _providerRepository;
        private readonly ICreditCardRepository _cardRepository;
        private readonly IProviderFactory _providerFactory;
        private readonly IAccountService _accountService;
        

        public CreditCardController(IProviderFactory providerFactory, IProviderRepository providerRepository, 
                                     IAccountService accountService, ICreditCardRepository cardRepository)
        {
            _providerRepository = providerRepository;
            _cardRepository = cardRepository;
            _providerFactory = providerFactory;
            _accountService = accountService;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("cards")]
        public async Task<IActionResult> Get()
        {
            var cards = await _cardRepository.GetAllCards();
            var result = AutoMapper.Mapper.Map<IEnumerable<CreditCardDto>>(cards);
            return Ok(result);
        }

        // Retrive cards from provider based on the user credentials (applicable for new cards)
        // We use Post insted of get in order to pass credintials in the body 
        [HttpPost("cards")]
        public async Task<IActionResult> GetCards([FromBody] ProviderCreatingDto providerDto)
        {
            IEnumerable<CreditCardDto> result;

            var providerDescriptor = new ProviderDoc
            {
                Name = providerDto.Name,
                Type = providerDto.Type,
                Credentials = providerDto.Credentials
            };

            try
            {
                var dataProvider = await _providerFactory.CreateDataProvider(providerDescriptor);
                var cards = (dataProvider as ICreditCardProvider)?.GetCards();
                dataProvider.Dispose();

                var newAccounts = FilterNewAccount(providerDto, cards);
                result = AutoMapper.Mapper.Map<IEnumerable<CreditCardDto>>(newAccounts);
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

        // Retrive cards from provider (applicable for existing cards)
        [HttpGet("users/{userId}/cards/update")]
        public async Task<IActionResult> UpdateCards(String userId)
        {
            IActionResult errorResult = null;
            var result = new List<CreditCardDto>();

            var providers = await _providerRepository.GetProviders(p => p.UserId.Equals(userId));
            var cardProviders = providers.Where(p => 
                p.Type.Equals(InstitutionType.Credit) 
                && HasOutdatedCards(p)).ToList();

            var tasks = UpdateProvidersInParallel(cardProviders, result);
            if (tasks.Any(t => !(t.Result is OkResult)))
            {
                errorResult = tasks.FirstOrDefault(t => !(t.Result is OkResult))?.Result;
            }
            return errorResult ?? Ok(result);
        }
        
        private List<Task<IActionResult>> UpdateProvidersInParallel(IList<ProviderDoc> cardProviders, List<CreditCardDto> result)
        {
            Barrier barrier = new Barrier(cardProviders.Count() + 1);
            List<Task<IActionResult>> tasks = new List<Task<IActionResult>>();

            foreach (var provider in cardProviders)
            {
                tasks.Add(Task<IActionResult>.Factory.StartNew(() =>
                {
                    try
                    {
                        var cards = _accountService.UpdateCreditCards(provider).Result;
                        var cardsDto = AutoMapper.Mapper.Map<IEnumerable<CreditCardDto>>(cards);
                        result.AddRange(cardsDto);
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
            return tasks;
        }
        
        private IEnumerable<CreditCard> FilterNewAccount(ProviderCreatingDto providerDto, IEnumerable<CreditCard> cards)
        {
            IEnumerable<CreditCard> newAccounts;
            var p = AutoMapper.Mapper.Map<ProviderDoc>(providerDto);
            var provider = _providerRepository.Find(p);
            if (provider != null)
            {
                newAccounts = (from card in cards
                               where !IsAccountExists(card)
                               select card).ToList();
            }
            else
            {
                newAccounts = cards;
            }
            return newAccounts;
        }

        private bool HasOutdatedCards(ProviderDoc provider)
        {
            return _cardRepository.GetCardsByProviderId(provider.Id).Result.ToList()
                .Any(a => a.UpdatedOn.AddHours(AccountService.UpdateInterval) < DateTime.Now);
        }

        private bool IsAccountExists(CreditCard card)
        {
            var result = _cardRepository.FindCardByCriteria(a => a.Id.Equals(card.Id));
            return result.Result != null;
        }
    }
}