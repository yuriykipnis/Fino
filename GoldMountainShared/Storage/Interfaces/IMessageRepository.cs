using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GoldMountainShared.Storage.Documents;

namespace GoldMountainShared.Storage.Interfaces
{
    public interface IMessageRepository
    {
        Task<IEnumerable<ContactMessageDoc>> GetAllMessages();

        Task<ContactMessageDoc> GetMessage(Guid id);

        Task<ContactMessageDoc> GetMessageByInternalId(string id);

        Task<IEnumerable<ContactMessageDoc>> GetMessagesByUserId(String id);

        Task AddMessage(ContactMessageDoc item);

        Task AddMessages(IEnumerable<ContactMessageDoc> items);

        Task<bool> RemoveMessage(Guid id);

        Task<bool> RemoveAllMessages();

        Task<ContactMessageDoc> FindMessageByCriteria(Expression<Func<ContactMessageDoc, bool>> filter);
    }
}