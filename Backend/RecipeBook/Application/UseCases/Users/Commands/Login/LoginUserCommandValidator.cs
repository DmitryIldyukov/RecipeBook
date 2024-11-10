using FluentValidation;

namespace Application.UseCases.Users.Commands.Login;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor( command => command.Login )
            .NotEmpty().WithMessage( "Логин обязателен." );

        RuleFor( command => command.Password )
            .NotEmpty().WithMessage( "Пароль не может быть пустым." )
            .MinimumLength( 8 ).WithMessage( "Пароль должен состоять минимум из 8 символов." );
    }
}
