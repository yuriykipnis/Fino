using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GoldMountainShared.Storage.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbContext _context = null;

        public UserRepository(IOptions<DbSettings> settings)
        {
            _context = new DbContext(settings);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            try
            {
                return await _context.Users.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<User> GetUser(Guid id)
        {
            try
            {
                return await _context.Users.Find(user => user.Id.Equals(id)).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<User> GetUserByInternalId(string id)
        {
            try
            {
                ObjectId internalId = GetInternalId(id);
                return await _context.Users.Find(user => user.InternalId == internalId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddUser(User user)
        {
            try
            {
                await _context.Users.InsertOneAsync(user);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdateUserEmail(Guid id, string email)
        {
            var filter = Builders<User>.Filter.Eq(s => s.Id, id);
            var update = Builders<User>.Update
                .Set(s => s.Email, email)
                .CurrentDate(s => s.UpdatedOn);

            try
            {
                UpdateResult actionResult = await _context.Users.UpdateOneAsync(filter, update);
                return actionResult.IsAcknowledged && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveUser(Guid id)
        {
            try
            {
                DeleteResult actionResult
                    = await _context.Users.DeleteOneAsync(Builders<User>.Filter.Eq("Id", id));

                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveAllUsers()
        {
            try
            {
                DeleteResult actionResult
                    = await _context.Users.DeleteManyAsync(_ => true);

                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        private ObjectId GetInternalId(string id)
        {
            if (!ObjectId.TryParse(id, out var internalId))
            {
                internalId = ObjectId.Empty;
            }

            return internalId;
        }
    }
}
