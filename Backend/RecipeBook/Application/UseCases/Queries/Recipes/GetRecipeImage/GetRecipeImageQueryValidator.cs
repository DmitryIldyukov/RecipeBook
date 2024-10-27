using FluentValidation;

namespace Application.UseCases.Queries.Recipes.GetRecipeImage;

public class GetRecipeImageQueryValidator : AbstractValidator<GetRecipeImageQuery>
{
    public GetRecipeImageQueryValidator()
    {
        RuleFor( q => q.RecipeId )
            .NotEmpty().WithMessage( "Id рецепта обязателен." );
    }
}
