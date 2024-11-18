using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.UseCases.Users.Queries.GetById;

public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryValidator( IUserRepository userRepository )
    {
        _userRepository = userRepository;

        RuleFor( query => query.Id )
            .GreaterThan( 0 ).WithMessage( "Идентификатор должен быть положительным числом." )
            .MustAsync( UserExists ).WithMessage( r => $"Пользователь не найден." );
    }

    private async Task<bool> UserExists( int userId, CancellationToken cancellationToken )
    {
        return await _userRepository.ContainsAsync( u => u.Id == userId );
    }
}