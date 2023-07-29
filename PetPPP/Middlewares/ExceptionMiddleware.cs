using Core.Exceptions;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Net;

namespace PetPPP.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate requestDelegate)
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
                    await HandleExceptionAsync(httpContext, uniqueIndexEx, HttpStatusCode.UnprocessableEntity);
                }
                if (ex is EntityNotFoundException entityNotFoundEx)
                {
                    await HandleExceptionAsync(httpContext, entityNotFoundEx, HttpStatusCode.NotFound);
                }
            }
        }

        public async Task HandleExceptionAsync(HttpContext httpContext, Exception ex, HttpStatusCode httpStatusCode)
        {
            var result = JsonConvert.SerializeObject(new
            {
                StatusCode = httpStatusCode,
                ErrorMessage = ex.Message
            });
            
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)httpStatusCode;
            await httpContext.Response.WriteAsync(result);
        }
    }
}
