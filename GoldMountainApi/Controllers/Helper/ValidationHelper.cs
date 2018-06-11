using System;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace GoldMountainApi.Controllers.Helper
{
    public class ValidationHelper : IValidationHelper
    {
        public bool ValidateUserPermissions(ClaimsPrincipal userContext, string userId)
        {
            string idRaw = userContext.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var id = idRaw?.Split('|').LastOrDefault();
            return !String.IsNullOrEmpty(id) && id.Equals(userId);
        }
    }
}
