using FluentValidation;

namespace Application.UseCases.Tags.Commands.Create;

public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
{
    public CreateTagCommandValidator()
    {
        RuleFor( s => s.Recipe )
            .NotNull().WithMessage( "Рецепт обязателен." );

        RuleFor( command => command.Name )
            .NotEmpty().WithMessage( "Тег не может быть пустым." )
            .MaximumLength( 20 ).WithMessage( "Максимальная длина тега 30 символов." );
    }
}