using System.Collections.Generic;
using System.IO;
using System.Threading;
using DataProvider.Providers.Cards.Amex.Dto;
using GoldMountainShared.Storage.Documents;
using Newtonsoft.Json;

namespace DataProvider.Providers.Cards.Amex
{
    public class AmexFakeApi : IAmexApi
    {
        private readonly bool _isValid;

        public bool IsReady => _isValid;

        public string UserId => "id";

        public AmexFakeApi(Provider providerDescriptor)
        {
            Thread.Sleep(500);
            if (providerDescriptor.Credentials.ContainsKey("id"))
            {
                _isValid = true;
            }
            else
            {
                _isValid = false;
            }
        }

        public CardListDeatils GetCards()
        {
            if (!_isValid)
            {
                return new CardListDeatils();
            }

            Thread.Sleep(100);

            var path = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\EasyBudgetService\Assets\Cards\Amex", "accounts.json");
            if (!File.Exists(path))
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), @"Assets\Cards\Amex", "accounts.json");
            }

            string json = File.ReadAllText(path);
            var accountsResponse = JsonConvert.DeserializeObject<CardListDeatils>(json);
            return accountsResponse;
        }

        public IEnumerable<Dto.CardTransaction> GetTransactions(long cardIndex, int month, int year)
        {
            Thread.Sleep(250);
            if (!_isValid)
            {
                return new List<Dto.CardTransaction>();
            }

            string json, path;
            
            switch (cardIndex)
            {
                case 1:
                    path = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\EasyBudgetService\Assets\Cards\Amex\Transactions", "transactions1.json");
                    if (!File.Exists(path))
                    {
                        path = Path.Combine(Directory.GetCurrentDirectory(), @"Assets\Cards\Amex\Transactions", "transactions1.json");
                    }
                    json = File.ReadAllText(path);
                    return AmexApi.RetriveExpensesInfo(json).Transactions;
                case 3:
                    path = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\EasyBudgetService\Assets\Cards\Amex\Transactions", "transactions2.json");
                    if (!File.Exists(path))
                    {
                        path = Path.Combine(Directory.GetCurrentDirectory(), @"Assets\Cards\Amex\Transactions", "transactions2.json");
                    }
                    json = File.ReadAllText(path);
                    return AmexApi.RetriveExpensesInfo(json).Transactions;
            }

            return new List<Dto.CardTransaction>();
        }

        public void Dispose()
        {
        }
    }
}
