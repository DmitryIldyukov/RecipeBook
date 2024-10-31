using FluentValidation;

namespace Application.UseCases.Tags.Commands.Create;

public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
{
    public CreateTagCommandValidator()
    {
        RuleFor( command => command.Name )
            .NotEmpty().WithMessage( "Тэг не может быть пустым." )
            .MaximumLength( 20 ).WithMessage( "Максимальная длина тэга 30 символов." );
    }
}