using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GoldMountainApi.Models;
using GoldMountainShared.Models.Bank;
using GoldMountainShared.Models.Credit;
using GoldMountainShared.Models.Shared;
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

        public async Task<CreditAccountDto> GetCreditAccount(Guid accountId)
        {
            var hostName = _configuration.GetSection("Host:Name").Value;
            var url = hostName + ":5002/api/CreditAccounts/" + accountId;
            HttpResponseMessage response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return new CreditAccountDto();
            }

            var res = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<CreditAccountDto>(res);
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

        public async Task<IEnumerable<BankAccount>> GetBankAccountsForUserId(String userId)
        {
            IEnumerable<BankAccount> result;
            var hostName = _configuration.GetSection("Host:Name").Value;
            var url = hostName + ":5002/api/users/" + userId + "/BankAccounts/";
            try
            {
                HttpResponseMessage response = await _client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return new List<BankAccount>();
                }

                var res = response.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<IList<BankAccount>>(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            return result;
        }

        public async Task<IEnumerable<BankAccount>> GetBankAccounts(IEnumerable<Guid> accounts)
        {
            return new List<BankAccount>();
        }

        public async Task<IEnumerable<CreditAccount>> GetCreditAccountsForUserId(String userId)
        {
            var hostName = _configuration.GetSection("Host:Name").Value;
            var url = hostName + ":5002/api/users/" + userId + "/CreditAccounts/";
            HttpResponseMessage response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return new List<CreditAccount>();
            }

            var res = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<IList<CreditAccount>>(res);
            return result;
        }

        private async Task<bool> ProcessProvider(Provider provider)
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
