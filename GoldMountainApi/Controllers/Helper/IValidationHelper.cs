using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GoldMountainApi.Controllers.Helper
{
    public interface IValidationHelper
    {
        bool ValidateUserPermissions(ClaimsPrincipal userContext, string userId);
    }
}
