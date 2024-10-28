using FluentValidation;

namespace Application.UseCases.Steps.Commands.Create;

public class CreateStepCommandValidator : AbstractValidator<CreateStepCommand>
{
    public CreateStepCommandValidator()
    {
        RuleFor( s => s.RecipeId )
            .NotNull().WithMessage( "Id рецепта обязателен." )
            .GreaterThan( 0 ).WithMessage( "Идентификатор должен быть положительным числом." );

        RuleFor( s => s.Description )
            .NotEmpty().WithMessage( "Описание шага обязательно." );
    }
}
