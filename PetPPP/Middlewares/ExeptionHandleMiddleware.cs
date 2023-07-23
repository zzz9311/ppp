using Core.Exceptions;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Net;

namespace PetPPP.Middlewares
{
    public class ExeptionHandleMiddleware
    {
        private readonly RequestDelegate _next;

        public ExeptionHandleMiddleware(RequestDelegate requestDelegate)
        {
            _next = requestDelegate;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
            }
            catch (Exception ex)
            {
                if (ex is UniqueIndexException uniqueIndexEx)
                {
                    await HandleUniqueIndexExceptionAsync(httpContext, uniqueIndexEx);
                }
            }
        }

        public async Task HandleUniqueIndexExceptionAsync(HttpContext httpContext, UniqueIndexException ex)
        {
            var result = JsonConvert.SerializeObject(new
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity,
                ErrorMessage = ex.Message,
            });
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = 422;
            await httpContext.Response.WriteAsync(result);
        }
    }
}
