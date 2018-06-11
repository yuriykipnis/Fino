using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoldMountainApi.Models;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GoldMountainApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UsersController
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet]
        public async Task<IEnumerable<User>> Get()
        {
            return await _userRepository.GetAllUsers();
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("{id}")]
        public async Task<User> Get(Guid id)
        {
            return await _userRepository.GetUser(id) ?? new User();
        }

        [HttpPost]
        public void Post([FromBody] UserDto newUser)
        {
            _userRepository.AddUser(new User
            {
                Id = new Guid(),
                Name = newUser.Name,
                Email = newUser.Email
            });
        }

        [HttpPut("{id}")]
        public void Put(Guid id, [FromBody]string email)
        {
            _userRepository.UpdateUserEmail(id, email);
        }

        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            _userRepository.RemoveUser(id);
        }
    }
}
