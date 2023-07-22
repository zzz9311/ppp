using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace PetPPP.Exeptions
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
                if (ex is SqlException)
                {

                }
            }
        }

        public async Task HandleSqlExceptionAsync(HttpContext httpContext, SqlException ex)
        {
            
        }
    }
}
