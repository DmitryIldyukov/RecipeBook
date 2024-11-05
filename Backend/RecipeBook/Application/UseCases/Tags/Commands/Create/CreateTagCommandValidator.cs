using FluentValidation;

namespace Application.UseCases.Tags.Commands.Create;

public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
{
    public CreateTagCommandValidator()
    {
        RuleFor(s => s.Recipe)
            .NotNull().WithMessage("Рецепт обязателен.");

        RuleFor( command => command.Name )
            .NotEmpty().WithMessage( "Тэг не может быть пустым." )
            .MaximumLength( 20 ).WithMessage( "Максимальная длина тэга 30 символов." );
    }
}