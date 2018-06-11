using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GoldMountainShared.Storage.Documents;

namespace GoldMountainShared.Storage.Interfaces
{
    public interface IProviderRepository
    {
        Task<Provider> GetProvider(Guid id);

        Task<IEnumerable<Provider>> GetProviders();
        
        Task<IEnumerable<Provider>> GetProviders(String userId);

        Task<IEnumerable<Provider>> GetProviders(Expression<Func<Provider, bool>> filter);

        Task AddProvider(Provider provider);

        Task<bool> RemoveProvider(String userId, String name);

        Task<bool> RemoveProviders(String userId);

        Task<bool> UpdateProvider(Guid id, Provider provider);

        Task<bool> RemoveAllProviders();

        Task<Provider> Find(Provider provider);

        Task<Provider> FindProviderByCriteria(Expression<Func<Provider, bool>> filter);
    }
}
