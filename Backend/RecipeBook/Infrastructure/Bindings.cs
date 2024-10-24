using Application.Common.PasswordHasher;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Infrastructure.Data;
using Infrastructure.EntityDefinitions.Ingredients;
using Infrastructure.EntityDefinitions.Recipes;
using Infrastructure.EntityDefinitions.Steps;
using Infrastructure.EntityDefinitions.Tags;
using Infrastructure.EntityDefinitions.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class Bindings
{
    public static IServiceCollection AddInfrastructure( this IServiceCollection services )
    {
        services.AddScoped<IRecipeRepository, RecipeRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<IIngredientRepository, IngredientRepository>();
        services.AddScoped<IStepRepository, StepRepository>();

        services.AddScoped<IPasswordHasher, PasswordHasher.PasswordHasher>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
