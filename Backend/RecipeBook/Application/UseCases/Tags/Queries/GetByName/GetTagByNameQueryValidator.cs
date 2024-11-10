using FluentValidation;

namespace Application.UseCases.Tags.Queries.GetByName;

public class GetTagByNameQueryValidator : AbstractValidator<GetTagByNameQuery>
{
    public GetTagByNameQueryValidator()
    {

    }
}
