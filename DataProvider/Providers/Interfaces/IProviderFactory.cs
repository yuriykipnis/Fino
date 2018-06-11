using System.Threading.Tasks;
using GoldMountainShared.Storage.Documents;

namespace DataProvider.Providers.Interfaces
{
    public interface IProviderFactory
    {
        Task<IAccountProvider> CreateDataProvider(Provider provider);
    }
}
