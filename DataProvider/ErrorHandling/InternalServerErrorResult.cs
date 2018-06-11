using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataProvider.ErrorHandling
{
    public class InternalServerErrorResult : IActionResult
    {
        private readonly String _message;
        public InternalServerErrorResult(String message)
        {
            _message = message;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = new ObjectResult(_message)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            await objectResult.ExecuteResultAsync(context);
        }
    }
}
