using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataProvider.ErrorHandling
{
    public class UnauthorizedActionResult : IActionResult
    {
        private readonly String _message;
        public UnauthorizedActionResult(String message)
        {
            _message = message;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = new ObjectResult(_message)
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };

            await objectResult.ExecuteResultAsync(context);
        }
    }
}
