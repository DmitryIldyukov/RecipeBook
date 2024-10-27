using FluentValidation;

namespace Application.UseCases.Commands.Steps.Create;

public class CreateStepCommandValidator : AbstractValidator<CreateStepCommand>
{
    public CreateStepCommandValidator()
    {
        RuleFor( s => s.RecipeId )
            .NotNull().WithMessage( "Id рецепта обязателен." );

        RuleFor( s => s.Description )
            .NotEmpty().WithMessage( "Описание шага обязательно." );
    }
}
