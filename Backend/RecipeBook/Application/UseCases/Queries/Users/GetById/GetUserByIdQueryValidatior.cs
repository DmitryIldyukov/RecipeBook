using FluentValidation;

namespace Application.UseCases.Queries.Users.GetById;

public class GetUserByIdQueryValidatior : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidatior()
    {
        RuleFor( query => query.Id )
            .GreaterThan( 0 ).WithMessage( "Идентификатор должен быть положительным числом." );
    }
}
