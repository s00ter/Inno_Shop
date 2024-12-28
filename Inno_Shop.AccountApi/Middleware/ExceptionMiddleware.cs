using System.Net;
using System.Text.Json;

namespace Inno_Shop.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            logger.LogError($"Something went wrong: {ex}");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = env.IsDevelopment()
            ? new ErrorDetail
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message,
                StackTrace = exception.StackTrace
            }
            : new ErrorDetail
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error."
            };

        return context.Response.WriteAsync(response.ToString());
    }
}

public class ErrorDetail
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string StackTrace { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}