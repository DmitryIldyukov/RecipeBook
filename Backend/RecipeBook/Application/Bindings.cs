using System.Reflection;
using Application.UseCases.Favorites;
using Application.UseCases.Ingredients;
using Application.UseCases.Likes;
using Application.UseCases.Recipes;
using Application.UseCases.Steps;
using Application.UseCases.Tags;
using Application.UseCases.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class Bindings
{
    public static IServiceCollection AddApplication( this IServiceCollection services )
    {
        services.AddAutoMapper( Assembly.GetExecutingAssembly() );

        services.AddIngredientBindings();
        services.AddFavoriteBindings();
        services.AddRecipeBindings();
        services.AddLikeBidings();
        services.AddUserBindings();
        services.AddStepBindings();
        services.AddTagBindings();

        return services;
    }
}
