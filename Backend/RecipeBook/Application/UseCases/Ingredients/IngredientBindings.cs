using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.UseCases.Ingredients.Commands.Create;
using Application.UseCases.Ingredients.Commands.UpdateIngredient;
using Application.UseCases.Ingredients.Commands.UpdateRecipeIngredients;
using Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.Ingredients;

public static class IngredientBindings
{
    public static void AddIngredientBindings( this IServiceCollection services )
    {
        services.AddScoped<IValidator<CreateIngredientCommand>, CreateIngredientCommandValidator>();
        services.AddScoped<IValidator<UpdateRecipeIngredientsCommand>, UpdateRecipeIngredientsCommandValidator>();
        services.AddScoped<IValidator<UpdateIngredientCommand>, UpdateIngredientCommandValidator>();

        services.AddScoped<ICommandHandler<CreateIngredientCommand, Result>, CreateIngredientCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateRecipeIngredientsCommand, Result>, UpdateRecipeIngredientsCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateIngredientCommand, Result>, UpdateIngredientCommandHandler>();
    }
}
