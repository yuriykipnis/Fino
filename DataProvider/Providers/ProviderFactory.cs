using System.Collections.Generic;
using System.Threading.Tasks;
using DataProvider.Providers.Banks.Hapoalim;
using DataProvider.Providers.Banks.Leumi;
using DataProvider.Providers.Banks.Tefahot;
using DataProvider.Providers.Cards.Amex;
using DataProvider.Providers.Cards.Cal;
using DataProvider.Providers.Interfaces;
using GoldMountainShared.Storage.Documents;

namespace DataProvider.Providers
{
    public class ProviderFactory : IProviderFactory
    {
        public async Task<IAccountProvider> CreateDataProvider(ProviderDoc provider)
        {
            IAccountProvider accountProvider = null;

            switch (provider.Name)
            {
                case "Bank Hapoalim":
                    //accountProvider = new HapoalimAccountProvider(provider, new HapoalimFileApi(provider));
                    accountProvider = new HapoalimAccountProvider(new HapoalimApi(provider.Credentials));
                    break;
                case "Bank Leumi":
                    accountProvider = new LeumiAccountProvider(new LeumiApi(provider));
                    break;
                case "Bank Mizrahi-Tefahot":
                    accountProvider = new TefahotAccountProvider(new TefahotApi(provider));
                    break;
                case "Amex":
                    accountProvider = new AmexProvider(new AmexApi(provider.Credentials));
                    //accountProvider = new AmexProvider(provider, new AmexFileApi(provider));
                    break;
                case "Visa Cal":
                    accountProvider = new CalProvider(new CalApi(provider.Credentials));
                    break;
                default: break;
            }

            return accountProvider;
        }
       
    }
}
