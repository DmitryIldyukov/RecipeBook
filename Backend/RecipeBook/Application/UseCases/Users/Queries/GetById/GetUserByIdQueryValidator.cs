using FluentValidation;

namespace Application.UseCases.Users.Queries.GetById;

public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor( query => query.Id )
            .GreaterThan( 0 ).WithMessage( "Идентификатор должен быть положительным числом." );
    }
}
