using System.Net;
using System.Text.Json;

namespace WebAPI.Middlewares;

public class ExceptionHandlerMiddleware( RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger )
{
    public async Task Invoke( HttpContext context )
    {
        try
        {
            await next( context );
        }
        catch ( Exception exception )
        {
            await HandleExceptionAsync( context, exception );
        }
    }

    private async Task HandleExceptionAsync( HttpContext context, Exception exception )
    {
        logger.LogError( exception, "An error occurred during request processing." );

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = ( int )HttpStatusCode.InternalServerError;

        string errorResponse = JsonSerializer.Serialize( new { error = "Внутренняя ошибка сервера." } );

#if DEBUG
        errorResponse = JsonSerializer.Serialize( new { error = exception.Message, stackTrace = exception.StackTrace } );
#endif

        await context.Response.WriteAsync( errorResponse );
    }
}