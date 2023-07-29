using Microsoft.AspNetCore.Mvc.Filters;

namespace PetPPP;

public class ExceptionFilter : IAsyncExceptionFilter
{
    public Task OnExceptionAsync(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        
        context.ExceptionHandled = true;
        return Task.CompletedTask;
    }
}