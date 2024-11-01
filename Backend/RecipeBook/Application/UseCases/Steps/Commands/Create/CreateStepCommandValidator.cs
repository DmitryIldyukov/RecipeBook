using FluentValidation;

namespace Application.UseCases.Steps.Commands.Create;

public class CreateStepCommandValidator : AbstractValidator<CreateStepCommand>
{
    public CreateStepCommandValidator()
    {
        RuleFor( s => s.RecipeId )
            .NotNull().WithMessage( "Идентификатор рецепта обязателен." );

        RuleFor( s => s.Description )
            .NotEmpty().WithMessage( "Описание шага обязательно." );
    }
}
