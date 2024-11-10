using System.Reflection;
using Application;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace WebAPI;

public class Program
{
    public static void Main( string[] args )
    {
        try
        {
            var builder = WebApplication.CreateBuilder( args );

            var connectionString = builder.Configuration.GetConnectionString( "MSSQLRecipeBook" );
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

            var app = builder.Build();

            if ( app.Environment.IsDevelopment() )
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

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
