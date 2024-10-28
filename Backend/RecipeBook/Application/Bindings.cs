using System.Reflection;
using Application.UseCases.Commands.Ingredients;
using Application.UseCases.Commands.Recipes;
using Application.UseCases.Commands.Steps;
using Application.UseCases.Commands.Tags;
using Application.UseCases.Commands.Users;
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
