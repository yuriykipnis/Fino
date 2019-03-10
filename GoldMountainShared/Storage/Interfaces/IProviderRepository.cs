using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GoldMountainShared.Storage.Documents;

namespace GoldMountainShared.Storage.Interfaces
{
    public interface IProviderRepository
    {
        Task<ProviderDoc> GetProvider(String id);

        Task<IEnumerable<ProviderDoc>> GetProviders();
        
        Task<IEnumerable<ProviderDoc>> GetProviders(String userId);

        Task<IEnumerable<ProviderDoc>> GetProviders(Expression<Func<ProviderDoc, bool>> filter);

        Task<String> AddProvider(ProviderDoc provider);

        Task<bool> RemoveProvider(String id);

        Task<bool> UpdateProvider(String id, ProviderDoc provider);

        Task<bool> RemoveAllProviders();

        Task<ProviderDoc> Find(ProviderDoc provider);

        Task<ProviderDoc> FindProviderByCriteria(Expression<Func<ProviderDoc, bool>> filter);
    }
}
