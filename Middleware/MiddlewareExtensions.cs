using System.Runtime.CompilerServices;

namespace DecoranestBacknd.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomeMiddleware(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ErrorHandlingMiddleware>();
           
            builder.UseMiddleware<RequestValidationMiddleware>();
            builder.UseMiddleware<LoggingMiddleware>();

            return builder;
        }
    }
}
