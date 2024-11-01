using FluentValidation;

namespace Application.UseCases.Recipes.Queries.GetFavoriteRecipes;

public class GetUserFavoriteRecipesQueryValidator : AbstractValidator<GetUserFavoriteRecipesQuery>
{
    public GetUserFavoriteRecipesQueryValidator()
    {
        RuleFor( f => f.UserId )
            .NotEmpty().WithMessage( "Идентификатор пользователя обязателен." );

        RuleFor( f => f.Page.PageNumber )
            .GreaterThan( -1 ).WithMessage( "Минимальный номер страницы 0." );

        RuleFor( f => f.Page.PageSize )
            .GreaterThan( 0 ).WithMessage( "Минимальный размер страницы 1." );
    }
}
