using FluentValidation;

namespace Application.UseCases.Recipes.Queries.GetRecipesByFilter;

public class GetRecipesByFilterQueryValidator : AbstractValidator<GetRecipesByFilterQuery>
{
    public GetRecipesByFilterQueryValidator()
    {

    }
}
