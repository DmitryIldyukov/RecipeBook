using FluentValidation;

namespace Application.UseCases.Recipes.Queries.GetRecipeImage;

public class GetRecipeImageQueryValidator : AbstractValidator<GetRecipeImageQuery>
{
    public GetRecipeImageQueryValidator()
    {
        RuleFor( q => q.RecipeId )
            .NotEmpty().WithMessage( "Идентификатор рецепта обязателен." )
            .GreaterThan( 0 ).WithMessage( "Идентификатор должен быть положительным числом." );
    }
}
