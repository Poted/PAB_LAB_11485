namespace API.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation("--> got request: {Method} {Path}", context.Request.Method, context.Request.Path);

        foreach (var header in context.Request.Headers.Where(h => h.Key == "User-Agent" || h.Key == "Accept"))
        {
            _logger.LogInformation("    Header: {Key}: {Value}", header.Key, header.Value);
        }

        await _next(context);

        _logger.LogInformation("<-- Ended with status: {StatusCode}", context.Response.StatusCode);
    }
}