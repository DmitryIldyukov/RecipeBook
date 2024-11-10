using System.Reflection;
using Application.UseCases.Ingredients;
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
        services.AddRecipeBindings();
        services.AddUserBindings();
        services.AddStepBindings();
        services.AddTagBindings();

        return services;
    }
}
