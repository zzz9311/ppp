using Microsoft.AspNetCore.Diagnostics;

namespace PetPPP.Extensions
{
    public static class ExceptionHandleMiddlewareExtensions
    {
        public static void UseExceptionHandlerMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
