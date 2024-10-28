using Application.Common.CQRS.Command;
using Application.Common.CQRS.Query;
using Application.UseCases.Commands.Recipes.Create;
using Application.UseCases.Queries.Recipes.Dtos;
using Application.UseCases.Queries.Recipes.GetRecipeImage;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.Commands.Recipes;

public static class RecipeBindings
{
    public static void AddRecipeBindings( this IServiceCollection services )
    {
        services.AddScoped<IValidator<CreateRecipeCommand>, CreateRecipeCommandValidator>();
        services.AddScoped<IValidator<GetRecipeImageQuery>, GetRecipeImageQueryValidator>();

        services.AddScoped<ICommandHandler<CreateRecipeCommand>, CreateRecipeCommandHandler>();

        services.AddScoped<IQueryHandler<GetRecipeImageQuery, GetImageQueryDto>, GetRecipeImageQueryHandler>();
    }
}
