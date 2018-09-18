using System;
using System.Collections.Specialized;
using System.Security.Authentication;
using GoldMountainApi.Controllers.Helper;
using GoldMountainApi.Services;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoldMountainApi.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    [Authorize]
    public class Contact : Controller
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IValidationHelper _validationHelper;

        public Contact(IMessageRepository messageRepository, IValidationHelper validationHelper)
        {
            _messageRepository = messageRepository;
            _validationHelper = validationHelper;
        }

        [HttpGet("contact/{userId}/Send")]
        public IActionResult Send(String userId, [FromBody] ContactMessage message)
        {
            if (!_validationHelper.ValidateUserPermissions(User, userId))
            {
                throw new AuthenticationException();
            }

            if (message == null)
            {
                throw new Exception("Something went wrong...");
            }

            _messageRepository.AddMessage(message);
            NotifyOnNewMessage(message);

            return Ok();
        }
    }
}