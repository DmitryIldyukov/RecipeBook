using FluentValidation;

namespace Application.UseCases.Queries.Recipes.GetById;

public class GetRecipeByIdQueryValidator : AbstractValidator<GetRecipeByIdQuery>
{
    public GetRecipeByIdQueryValidator()
    {
    }
}
