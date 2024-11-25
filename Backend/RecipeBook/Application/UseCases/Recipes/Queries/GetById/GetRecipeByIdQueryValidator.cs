using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.UseCases.Recipes.Queries.GetById;

public class GetRecipeByIdQueryValidator : AbstractValidator<GetRecipeByIdQuery>
{
    private readonly IRecipeRepository _recipeRepository;

    public GetRecipeByIdQueryValidator( IRecipeRepository recipeRepository )
    {
        _recipeRepository = recipeRepository;

        RuleFor( s => s.RecipeId )
           .NotNull().WithMessage( "Идентификатор рецепта обязателен." )
           .GreaterThan( 0 ).WithMessage( "Идентификатор должен быть положительным числом." )
           .MustAsync( RecipeExists ).WithMessage( r => $"Рецепт с Id {r.RecipeId} не найден." );
    }

    private async Task<bool> RecipeExists( int recipeId, CancellationToken cancellationToken )
    {
        return await _recipeRepository.ContainsAsync( r => r.Id == recipeId );
    }
}
