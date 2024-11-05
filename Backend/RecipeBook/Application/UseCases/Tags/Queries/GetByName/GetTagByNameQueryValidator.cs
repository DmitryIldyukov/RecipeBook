using FluentValidation;

namespace Application.UseCases.Tags.Queries.GetByName;

public class GetTagByNameQueryValidator : AbstractValidator<GetTagByNameQuery>
{
    public GetTagByNameQueryValidator()
    {
        RuleFor( f => f.Tag )
           .NotEmpty().WithMessage( "Тег обязателен." );
    }
}
