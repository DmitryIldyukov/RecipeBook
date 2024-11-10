using FluentValidation;

namespace Application.UseCases.Recipes.Queries.GetById;

public class GetRecipeByIdQueryValidator : AbstractValidator<GetRecipeByIdQuery>
{
    public GetRecipeByIdQueryValidator()
    {
    }
}
