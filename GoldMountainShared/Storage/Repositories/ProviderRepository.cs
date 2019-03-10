using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GoldMountainShared.Storage.Repositories
{
    public class ProviderRepository : IProviderRepository
    {
        private readonly DbContext _context;

        public ProviderRepository(IOptions<DbSettings> settings)
        {
            _context = new DbContext(settings);
        }

        public async Task<ProviderDoc> GetProvider(String id)
        {
            try
            {
                return await _context.Providers.Find(provider => provider.Id.Equals(id)).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<IEnumerable<ProviderDoc>> GetProviders()
        {
            try
            {
                return await _context.Providers.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<IEnumerable<ProviderDoc>> GetProviders(String userId)
        {
            try
            {
                return await _context.Providers.Find(provider => provider.UserId.Equals(userId)).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<IEnumerable<ProviderDoc>> GetProviders(Expression<Func<ProviderDoc, bool>> filter)
        {
            try
            {
                return await _context.Providers.Find(filter).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<String> AddProvider(ProviderDoc provider)
        {
            try
            {
                await _context.Providers.InsertOneAsync(provider);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }

            return provider.Id;
        }

        public async Task<bool> RemoveProvider(String id)
        {
            try
            {
                DeleteResult actionResult
                    = await _context.Providers.DeleteOneAsync(Builders<ProviderDoc>.Filter.Eq("Id", id));

                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdateProvider(String id, ProviderDoc provider)
        {
            try
            {
                ReplaceOneResult actionResult = await _context.Providers.ReplaceOneAsync(
                    p => p.Id.Equals(id),
                    provider, new UpdateOptions { IsUpsert = true });

                return actionResult.IsAcknowledged && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveAllProviders()
        {
            try
            {
                DeleteResult actionResult
                    = await _context.Providers.DeleteManyAsync(new BsonDocument());

                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<ProviderDoc> Find(ProviderDoc provider)
        {
            try
            {
                var providers = await _context.Providers.Find(
                    p => p.Name.Equals(provider.Name)
                    && p.UserId.Equals(provider.UserId)
                ).ToListAsync();

                var result = (from p in providers
                    where EqualCredentials(p.Credentials, provider.Credentials)
                    select p).FirstOrDefault();

                return result;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<ProviderDoc> FindProviderByCriteria(Expression<Func<ProviderDoc, bool>> filter)
        {
            try
            {
                return await _context.Providers.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool EqualCredentials(IDictionary<string, string> dic1, IDictionary<string, string> dic2)
        {
            var firstNotSecond = dic1.Values.Except(dic2.Values).ToList();
            var secondNotFirst = dic2.Values.Except(dic1.Values).ToList();

            return !firstNotSecond.Any() && !secondNotFirst.Any();
        }
    }
}
