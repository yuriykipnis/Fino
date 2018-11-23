using System;
using System.Collections.Specialized;
using System.Security.Authentication;
using System.Threading.Tasks;
using GoldMountainApi.Controllers.Helper;
using GoldMountainApi.Models;
using GoldMountainApi.Services;
using GoldMountainShared.Models.Shared;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoldMountainApi.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    [Authorize]
    public class ContactController : Controller
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IValidationHelper _validationHelper;
        private readonly IEmailHelper _emailHelper;

        public ContactController(IMessageRepository messageRepository, IValidationHelper validationHelper,
                                 IEmailHelper emailHelper)
        {
            _messageRepository = messageRepository;
            _validationHelper = validationHelper;
            _emailHelper = emailHelper;
        }

        [HttpPost("contact/{userId}/send")]
        public async Task<IActionResult> Send(String userId, [FromBody] ContactMessageDto messageDto)
        {
            if (!_validationHelper.ValidateUserPermissions(User, userId))
            {
                throw new AuthenticationException();
            }

            if (messageDto == null)
            {
                throw new Exception("Something went wrong...");
            }

            try
            {
                var message = AutoMapper.Mapper.Map<ContactMessage>(messageDto);
                message.UserId = userId;
                message.Id = Guid.NewGuid();
                await _messageRepository.AddMessage(message);
                await _emailHelper.SendMessage(message);
            }
            catch (Exception e)
            {
                throw;
            }

            return Ok();
        }
    }
}