using System.Net;
using System.Text.Json;

namespace WebAPI.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware( RequestDelegate next ) =>
        _next = next;

    public async Task Invoke( HttpContext context )
    {
        try
        {
            await _next( context );
        }
        catch ( Exception exception )
        {
            await HandleExceptionAsync( context, exception );
        }
    }

    private async Task HandleExceptionAsync( HttpContext context, Exception exception )
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = ( int )HttpStatusCode.InternalServerError;

        string errorResponse = JsonSerializer.Serialize( new { error = "Внутренняя ошибка сервера." } );

#if DEBUG
        errorResponse = JsonSerializer.Serialize( new { error = exception.Message, stackTrace = exception.StackTrace } );
#endif

        await context.Response.WriteAsync( errorResponse );
    }
}