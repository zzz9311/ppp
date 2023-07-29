using Microsoft.AspNetCore.Mvc.Filters;

namespace MailService;

public class ExceptionFilter : IAsyncExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger;

    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        _logger = logger;
    }

    public Task OnExceptionAsync(ExceptionContext context)
    {
        var error = context.Exception;
        _logger.LogError(error, "Unexpected error");
        
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.ExceptionHandled = true;
        return Task.CompletedTask;
    }
}