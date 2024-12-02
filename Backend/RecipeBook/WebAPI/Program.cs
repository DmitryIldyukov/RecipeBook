using System.Reflection;
using Application;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.JwtProviders;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WebAPI.Extensions;
using WebAPI.Middlewares;

namespace WebAPI;

public class Program
{
    public static void Main( string[] args )
    {
        try
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder( args );

            builder.AddSerilogLogging();

            string connectionString = builder.Configuration.GetConnectionString( "MSSQLRecipeBook" );
            builder.Services.AddDbContext<RecipeBookDbContext>( options =>
            {
                options.UseSqlServer( connectionString, b => b.MigrationsAssembly( "Infrastructure.Migrations" ) );
            } );

            builder.Services
                .AddApplication()
                .AddInfrastructure();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddAutoMapper( Assembly.GetExecutingAssembly() );

            builder.Services.Configure<JwtOptions>( builder.Configuration.GetSection( nameof( JwtOptions ) ) );

            builder.Services.AddApiAuthentication( builder.Configuration );

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwagger();

            WebApplication app = builder.Build();

            if ( app.Environment.IsDevelopment() )
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<RequestLoggingMiddleware>();

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseCookiePolicy( new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always,
            } );

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
        catch ( Exception ex )
        {
            Log.Fatal( ex.Message );
            Log.Information( "Сервер неожиданно завершил работу." );
        }
        finally
        {
            Log.Information( "Сервер отключается..." );
        }
    }
}
