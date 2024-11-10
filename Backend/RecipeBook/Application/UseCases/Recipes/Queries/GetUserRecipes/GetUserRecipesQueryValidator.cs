using FluentValidation;

namespace Application.UseCases.Recipes.Queries.GetUserRecipes;

public class GetUserRecipesQueryValidator : AbstractValidator<GetUserRecipesQuery>
{
    public GetUserRecipesQueryValidator()
    {
        RuleFor( q => q.UserId )
            .NotEmpty().WithMessage( "Идентификатор пользователя обязателен." )
            .GreaterThan( 0 ).WithMessage( "Идентификатор пользователя должен быть положительным числом." );
    }
}
