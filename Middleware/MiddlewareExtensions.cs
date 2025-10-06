using System.Runtime.CompilerServices;

namespace DecoranestBacknd.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomeMiddleware(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<LoggingMiddleware>();
            builder.UseMiddleware<ErrorHandlingMiddleware>();
           
            builder.UseMiddleware<RequestValidationMiddleware>();
            return builder;
        }
    }
}
