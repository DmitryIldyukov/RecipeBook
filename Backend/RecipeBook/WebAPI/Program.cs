using System.Reflection;
using Application;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using WebAPI.Middlewares;

namespace WebAPI;

public class Program
{
    public static void Main( string[] args )
    {
        try
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder( args );

            string connectionString = builder.Configuration.GetConnectionString( "MSSQLRecipeBook" );
            builder.Services.AddDbContext<RecipeBookDbContext>( options =>
            {
                options.UseSqlServer( connectionString, b => b.MigrationsAssembly( "Infrastructure.Migrations" ) );
            } );

            builder.Services
                .AddApplication()
                .AddInfrastructure();

            builder.Services.AddAutoMapper( Assembly.GetExecutingAssembly() );

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            WebApplication app = builder.Build();

            if ( app.Environment.IsDevelopment() )
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
        catch ( Exception ex )
        {
            Console.WriteLine( ex.Message );
            Console.WriteLine( "Сервер неожиданно завершил работу." );
        }
        finally
        {
            Console.WriteLine( "Сервер отключается..." );
        }
    }
}
