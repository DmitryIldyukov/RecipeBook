using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.UseCases.Users.Commands.Login;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    private readonly IUserRepository _userRepository;

    public LoginUserCommandValidator( IUserRepository userRepositor )
    {
        _userRepository = userRepositor;

        RuleFor( command => command.Login )
            .NotEmpty().WithMessage( "Логин обязателен." )
            .MustAsync( UserExists ).WithMessage( r => $"Пользователь не найден." );

        RuleFor( command => command.Password )
            .NotEmpty().WithMessage( "Пароль не может быть пустым." )
            .MinimumLength( 8 ).WithMessage( "Пароль должен состоять минимум из 8 символов." );
    }

    private async Task<bool> UserExists( string login, CancellationToken cancellationToken )
    {
        return await _userRepository.ContainsAsync( u => u.Login == login );
    }
}
