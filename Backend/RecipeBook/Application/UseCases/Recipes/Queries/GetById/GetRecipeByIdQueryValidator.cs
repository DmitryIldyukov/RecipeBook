using FluentValidation;

namespace Application.UseCases.Recipes.Queries.GetById;

public class GetRecipeByIdQueryValidator : AbstractValidator<GetRecipeByIdQuery>
{
    public GetRecipeByIdQueryValidator()
    {
        RuleFor( q => q.RecipeId )
            .NotEmpty().WithMessage( "Идентификатор рецепта обязателен." )
            .GreaterThan( 0 ).WithMessage( "Идентификатор должен быть положительным числом." );
    }
}
