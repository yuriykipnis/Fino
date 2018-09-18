using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GoldMountainShared.Storage.Documents;

namespace GoldMountainShared.Storage.Interfaces
{
    public interface IMessageRepository
    {
        Task<IEnumerable<ContactMessage>> GetAllMessages();

        Task<ContactMessage> GetMessage(Guid id);

        Task<ContactMessage> GetMessageByInternalId(string id);

        Task<IEnumerable<ContactMessage>> GetMessagesByUserId(String id);

        Task AddMessage(ContactMessage item);

        Task AddMessages(IEnumerable<ContactMessage> items);

        Task<bool> RemoveMessage(Guid id);

        Task<bool> RemoveAllMessages();

        Task<ContactMessage> FindMessageByCriteria(Expression<Func<ContactMessage, bool>> filter);
    }
}