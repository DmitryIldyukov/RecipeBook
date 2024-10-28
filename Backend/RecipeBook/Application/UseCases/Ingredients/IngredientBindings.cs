using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.UseCases.Ingredients.Commands.Create;
using Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.Ingredients;

public static class IngredientBindings
{
    public static void AddIngredientBindings( this IServiceCollection services )
    {
        services.AddScoped<IValidator<CreateIngredientCommand>, CreateIngredientCommandValidator>();

        services.AddScoped<ICommandHandler<CreateIngredientCommand, ResultT<Ingredient>>, CreateIngredientCommandHandler>();
    }
}
