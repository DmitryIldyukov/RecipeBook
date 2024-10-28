using Application.Common.CQRS.Command;
using Application.UseCases.Commands.Ingredients.Create;
using Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.Commands.Ingredients;

public static class IngredientBindings
{
    public static void AddIngredientBindings( this IServiceCollection services )
    {
        services.AddScoped<IValidator<CreateIngredientCommand>, CreateIngredientCommandValidator>();

        services.AddScoped<ICommandHandler<CreateIngredientCommand, Ingredient>, CreateIngredientCommandHandler>();
    }
}
