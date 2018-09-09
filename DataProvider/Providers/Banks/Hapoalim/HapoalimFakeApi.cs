using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using DataProvider.Providers.Banks.Hapoalim.Dto;
using GoldMountainShared.Storage.Documents;
using Newtonsoft.Json;

namespace DataProvider.Providers.Banks.Hapoalim
{
    public class HapoalimFakeApi : IHapoalimApi
    {
        private readonly bool _isReady;
        public bool IsReady => _isReady;
        public string UserId
        {
            get { return "id"; }
        }

        public HapoalimFakeApi(Provider providerDescriptor)
        {
            if (providerDescriptor == null || providerDescriptor.Credentials.Count == 0)
            {
                throw new ArgumentNullException(nameof(providerDescriptor));
            }

            Thread.Sleep(300);
            if (providerDescriptor.Credentials.ContainsKey("password"))
            {
                _isReady = true;
            }
            else
            {
                _isReady = false;
            }
        }

        public IEnumerable<HapoalimAccountResponse> GetAccountsData()
        {
            Thread.Sleep(500);

            if (!IsReady)
            {
                return Enumerable.Empty<HapoalimAccountResponse>();
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\EasyBudgetService\Assets\Banks\Hapoalim", "accounts.json");
            if (!File.Exists(path))
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), @"Assets\Banks\Hapoalim", "accounts.json");
            }

            string json = File.ReadAllText(path);
            var accountsResponse = JsonConvert.DeserializeObject<IList<HapoalimAccountResponse>>(json);
            return accountsResponse.AsEnumerable();
        }

        public HapoalimTransactionsResponse GetTransactions(HapoalimAccountResponse account, DateTime startTime, DateTime endTime)
        {
            Thread.Sleep(500);
            if (!_isReady)
            {
                return new HapoalimTransactionsResponse();
            }

            
            var hapoalimAssetsPath =
                Path.Combine(Directory.GetCurrentDirectory(), @"Assets\Banks\Hapoalim\Transactions");

            string json = String.Empty;
            string path = String.Empty;
            switch (account.BranchNumber)
            {
                case 135:
                    path = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\EasyBudgetService\Assets\Banks\Hapoalim\Transactions", "transactions1.json");
                    if (!File.Exists(path))
                    {
                        path = Path.Combine(Directory.GetCurrentDirectory(), @"Assets\Banks\Hapoalim\Transactions", "transactions1.json");
                    }

                    json = File.ReadAllText(path);
                    break;
                case 345:
                    path = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\EasyBudgetService\Assets\Banks\Hapoalim\Transactions", "transactions2.json");
                    if (!File.Exists(path))
                    {
                        path = Path.Combine(Directory.GetCurrentDirectory(), @"Assets\Banks\Hapoalim\Transactions", "transactions2.json");
                    }

                    json = File.ReadAllText(path);
                    break;
                case 545:
                    path = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\EasyBudgetService\Assets\Banks\Hapoalim\Transactions", "transactions3.json");
                    if (!File.Exists(path))
                    {
                        path = Path.Combine(Directory.GetCurrentDirectory(), @"Assets\Banks\Hapoalim\Transactions", "transactions3.json");
                    }

                    json = File.ReadAllText(path);
                    break;
            }

            var transactionResponce = JsonConvert.DeserializeObject<HapoalimTransactionsResponse>(json);
            return transactionResponce;
        }

        public HapoalimMortgagesResponse GetMortgages(HapoalimAccountResponse account)
        {
            throw new NotImplementedException();
        }

        public HapoalimBalanceResponse GetBalance(HapoalimAccountResponse account)
        {

            Thread.Sleep(500);
            if (!IsReady)
            {
                return new HapoalimBalanceResponse();
            }

            var hapoalimAssetsPath =
                Path.Combine(Directory.GetCurrentDirectory(), @"Assets\Banks\Hapoalim\Accounts");

            string json = String.Empty;
            switch (account.BranchNumber)
            {
                case 135:
                    json = File.ReadAllText(hapoalimAssetsPath + "/balance1.json");
                    break;
                case 345:
                    json = File.ReadAllText(hapoalimAssetsPath + "/balance2.json");
                    break;
                case 545:
                    json = File.ReadAllText(hapoalimAssetsPath + "/balance3.json");
                    break;
            }

            return JsonConvert.DeserializeObject<HapoalimBalanceResponse>(json);
        }

        public void Dispose()
        {
        }
    }
}
