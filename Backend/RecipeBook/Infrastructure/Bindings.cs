using Application.Common.FileHelper;
using Application.Common.JwtProvider;
using Application.Common.PasswordHasher;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Infrastructure.Data;
using Infrastructure.EntityDefinitions.Favorites;
using Infrastructure.EntityDefinitions.Ingredients;
using Infrastructure.EntityDefinitions.Likes;
using Infrastructure.EntityDefinitions.Recipes;
using Infrastructure.EntityDefinitions.Steps;
using Infrastructure.EntityDefinitions.Tags;
using Infrastructure.EntityDefinitions.Users;
using Infrastructure.JwtProviders;
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
        services.AddScoped<IFavoriteRepository, FavoriteRepository>();
        services.AddScoped<ILikeRepository, LikeRepository>();

        services.AddScoped<IPasswordHasher, PasswordHasher.PasswordHasher>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IFileHelper, FileHelper.FileHelper>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
