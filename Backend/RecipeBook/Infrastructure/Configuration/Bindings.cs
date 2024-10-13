using Application.Interfaces;
using Application.Interfaces.Repositories;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configuration;

public static class Bindings
{
    public static IServiceCollection AddInfrastructure( this IServiceCollection services )
    {
        services.AddScoped<IRecipeRepository, RecipeRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
