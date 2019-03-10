using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GoldMountainShared.Storage.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DbContext _context = null;

        public MessageRepository(IOptions<DbSettings> settings)
        {
            _context = new DbContext(settings);
        }

        public async Task<IEnumerable<ContactMessageDoc>> GetAllMessages()
        {
            try
            {
                return await _context.Messages.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<ContactMessageDoc> GetMessage(Guid id)
        {
            try
            {
                return await _context.Messages.Find(message => message.Id.Equals(id)).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<ContactMessageDoc> FindMessageByCriteria(Expression<Func<ContactMessageDoc, bool>> filter)
        {
            try
            {
                return await _context.Messages.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<ContactMessageDoc> GetMessageByInternalId(string id)
        {
            try
            {
                ObjectId internalId = GetInternalId(id);
                return await _context.Messages.Find(message => message.InternalId == internalId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<IEnumerable<ContactMessageDoc>> GetMessagesByUserId(String userId)
        {
            try
            {
                return await _context.Messages.Find(message => message.UserId.Equals(userId)).ToListAsync();

            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddMessage(ContactMessageDoc item)
        {
            try
            {
                await _context.Messages.InsertOneAsync(item);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddMessages(IEnumerable<ContactMessageDoc> items)
        {
            try
            {
                await _context.Messages.InsertManyAsync(items);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveMessage(Guid id)
        {
            try
            {
                DeleteResult actionResult
                    = await _context.Messages.DeleteOneAsync(Builders<ContactMessageDoc>.Filter.Eq("Id", id));

                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveAllMessages()
        {
            try
            {
                DeleteResult actionResult
                    = await _context.Messages.DeleteManyAsync(_ => true);

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
