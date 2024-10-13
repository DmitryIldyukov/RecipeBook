using Application.Interfaces;
using Application.Interfaces.Repositories;
using Infrastructure.Data;
using Infrastructure.EntityDefinitions.Recipes;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class Bindings
{
    public static IServiceCollection AddInfrastructure( this IServiceCollection services )
    {
        services.AddScoped<IRecipeRepository, RecipeRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
