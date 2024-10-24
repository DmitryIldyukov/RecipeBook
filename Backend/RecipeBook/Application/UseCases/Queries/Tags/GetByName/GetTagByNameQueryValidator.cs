using FluentValidation;

namespace Application.UseCases.Queries.Tags.GetByName;

public class GetTagByNameQueryValidator : AbstractValidator<GetTagByNameQuery>
{
    public GetTagByNameQueryValidator()
    {
        
    }
}
