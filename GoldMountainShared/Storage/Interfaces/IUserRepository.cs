using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoldMountainShared.Storage.Documents;

namespace GoldMountainShared.Storage.Interfaces
{
    public interface IUserRepository
    {

        Task<IEnumerable<UserDoc>> GetAllUsers();

        Task<UserDoc> GetUser(String id);

        Task AddUser(UserDoc user);

        Task<bool> UpdateUserEmail(String id, string email);

        Task<bool> RemoveUser(String id);

        Task<bool> RemoveAllUsers();

    }
}
