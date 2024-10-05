using FluentValidation;

namespace Application.UseCases.Queries.Recipe.GetById;

public class GetRecipeByIdQueryValidator : AbstractValidator<GetRecipeByIdQuery>
{
    public GetRecipeByIdQueryValidator()
    {
        
    }
}
