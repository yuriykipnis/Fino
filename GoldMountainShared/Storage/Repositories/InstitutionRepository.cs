using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GoldMountainShared.Storage.Repositories
{
    public class InstitutionRepository :  IInstitutionRepository
    {
        private readonly DbContext _context = null;

        public InstitutionRepository(IOptions<DbSettings> settings)
        {
            _context = new DbContext(settings);
        }

        public async Task<IEnumerable<Institution>> GetInstitutions()
        {
            try
            {
                return await _context.Institutions.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddInstitution(Institution item)
        {
            try
            {
                await _context.Institutions.InsertOneAsync(item);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveAllInstitutions()
        {
            try
            {
                DeleteResult actionResult = await _context.Institutions.DeleteManyAsync(_ => true);
                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }
    }
}
