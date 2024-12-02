using Serilog;

namespace WebAPI.Extensions;

public static class LoggingExtensions
{
    public static IHostBuilder AddSerilogLogging( this WebApplicationBuilder builder )
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration( builder.Configuration )
            .CreateLogger();

        return builder.Host.UseSerilog();
    }
}
