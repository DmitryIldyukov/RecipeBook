using Application.Common.CQRS.Command;
using Application.Common.CQRS.Query;
using Application.Common.Result;
using Application.UseCases.Recipes.Commands.Create;
using Application.UseCases.Recipes.Dtos;
using Application.UseCases.Recipes.Queries.GetRecipeImage;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.Recipes;

public static class RecipeBindings
{
    public static void AddRecipeBindings( this IServiceCollection services )
    {
        services.AddScoped<IValidator<CreateRecipeCommand>, CreateRecipeCommandValidator>();
        services.AddScoped<IValidator<GetRecipeImageQuery>, GetRecipeImageQueryValidator>();

        services.AddScoped<ICommandHandler<CreateRecipeCommand, Result>, CreateRecipeCommandHandler>();

        services.AddScoped<IQueryHandler<GetRecipeImageQuery, ResultT<GetImageQueryDto>>, GetRecipeImageQueryHandler>();
    }
}
