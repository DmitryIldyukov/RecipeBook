using FluentValidation;

namespace Application.UseCases.Recipes.Queries.GetRecipesByFilter;

public class GetRecipesByFilterQueryValidator : AbstractValidator<GetRecipesByFilterQuery>
{
    public GetRecipesByFilterQueryValidator()
    {
        RuleFor( f => f.Page.PageNumber )
            .GreaterThan( 0 ).WithMessage( "Минимальный номер страницы 1." );

        RuleFor( f => f.Page.PageSize )
            .GreaterThan( 0 ).WithMessage( "Минимальный размер страницы 1." );
    }
}
