using System.Diagnostics;

namespace WebAPI.Middlewares;

public class RequestLoggingMiddleware( RequestDelegate next, ILogger<RequestLoggingMiddleware> logger )
{
    public async Task InvokeAsync( HttpContext httpContext )
    {
        HttpRequest request = httpContext.Request;
        string method = request.Method;
        string path = request.Path;
        string userId = httpContext.User?.FindFirst( "userId" )?.Value ?? "Anonymous";

        logger.LogInformation( "Request from User {UserId}: {Method} {Path}", userId, method, path );

        Stopwatch stopwatch = Stopwatch.StartNew();
        await next( httpContext );
        stopwatch.Stop();

        int statusCode = httpContext.Response.StatusCode;
        logger.LogInformation(
            "Response from User {@UserId}: {Method} {Path} StatusCode: {StatusCode} Duration: {Duration}ms",
            userId,
            method,
            path,
            statusCode,
            stopwatch.ElapsedMilliseconds
        );
    }
}
