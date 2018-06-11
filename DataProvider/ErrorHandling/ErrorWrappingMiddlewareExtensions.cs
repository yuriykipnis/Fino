using Microsoft.AspNetCore.Builder;

namespace DataProvider.ErrorHandling
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorWrapping(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorWrappingMiddleware>();
        }
    }
}
