using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GoldMountainApi.Models;
using GoldMountainShared.Dto.Bank;
using GoldMountainShared.Dto.Credit;
using GoldMountainShared.Dto.Shared;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace GoldMountainApi.Services
{
    public class DataService : IDataService
    {
        public readonly IConfiguration _configuration;

        private readonly IBankAccountRepository _accountRepository;
        private readonly IProviderRepository _providerRepository;
        private readonly HttpClient _client;

        public DataService(IProviderRepository providerRepository, IBankAccountRepository accountRepository, IConfiguration configuration)
        {
            _configuration = configuration;
            _providerRepository = providerRepository;
            _accountRepository = accountRepository;
            _client = new HttpClient();
        }

        public async Task<bool> UpdateAccount(Guid accountId)
        {
            var account = await _accountRepository.GetAccount(accountId);
            var provider = await _providerRepository.GetProvider(account.ProviderId);
            return await ProcessProvider(provider);
        }

        public async Task<BankAccountDto> GetBankAccount(Guid accountId)
        {
            var hostName = _configuration.GetSection("Host:Name").Value;
            var url = hostName + ":5002/api/BankAccounts/" + accountId;
            HttpResponseMessage response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return new BankAccountDto();
            }

            var res = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<BankAccountDto>(res);
            return result;
        }

        public async Task<CreditCardDto> GetCreditAccount(Guid accountId)
        {
            var hostName = _configuration.GetSection("Host:Name").Value;
            var url = hostName + ":5002/api/CreditAccounts/" + accountId;
            HttpResponseMessage response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return new CreditCardDto();
            }

            var res = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<CreditCardDto>(res);
            return result;
        }

        public async Task<IEnumerable<TransactionDto>> GetTransactionsForAccount(Guid accountId, DateTime period)
        {
            var hostName = _configuration.GetSection("Host:Name").Value;
            var url = hostName + ":5002/api/accounts/" + accountId + "/transactions?year=" + period.Year + "&month=" + period.Month;
            HttpResponseMessage response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return new List<TransactionDto>();
            }

            var res = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<IList<TransactionDto>>(res);
            return result;
        }

        public async Task<IEnumerable<BankAccountDoc>> GetBankAccountsForUserId(String userId)
        {
            IEnumerable<BankAccountDoc> result;
            var hostName = _configuration.GetSection("Host:Name").Value;
            var url = hostName + ":5002/api/users/" + userId + "/BankAccounts/";
            try
            {
                HttpResponseMessage response = await _client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return new List<BankAccountDoc>();
                }

                var res = response.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<IList<BankAccountDoc>>(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            return result;
        }

        public async Task<IEnumerable<BankAccountDoc>> GetBankAccounts(IEnumerable<Guid> accounts)
        {
            return new List<BankAccountDoc>();
        }

        public async Task<IEnumerable<CreditCardDoc>> GetCreditAccountsForUserId(String userId)
        {
            var hostName = _configuration.GetSection("Host:Name").Value;
            var url = hostName + ":5002/api/users/" + userId + "/CreditAccounts/";
            HttpResponseMessage response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return new List<CreditCardDoc>();
            }

            var res = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<IList<CreditCardDoc>>(res);
            return result;
        }

        private async Task<bool> ProcessProvider(ProviderDoc provider)
        {
            var hostName = _configuration.GetSection("Host:Name").Value;
            HttpResponseMessage response = await _client.GetAsync(hostName + ":5002/accounts/" +"");

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var product = await response.Content.ReadAsStringAsync();
            return true;
        }
    }
     
}
