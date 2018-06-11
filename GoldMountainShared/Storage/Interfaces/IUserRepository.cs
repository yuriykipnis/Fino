using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoldMountainShared.Storage.Documents;

namespace GoldMountainShared.Storage.Interfaces
{
    public interface IUserRepository
    {

        Task<IEnumerable<User>> GetAllUsers();

        Task<User> GetUser(Guid id);

        Task<User> GetUserByInternalId(string id);

        Task AddUser(User user);

        Task<bool> UpdateUserEmail(Guid id, string email);

        Task<bool> RemoveUser(Guid id);

        Task<bool> RemoveAllUsers();

    }
}
