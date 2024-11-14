using FluentValidation;

namespace Application.UseCases.RefreshTokens.Commands.Refresh;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor( command => command.RefreshToken )
            .NotEmpty().WithMessage( "Токен не передан." );
    }
}
