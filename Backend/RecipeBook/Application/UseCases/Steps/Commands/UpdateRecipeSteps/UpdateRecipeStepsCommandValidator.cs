using FluentValidation;

namespace Application.UseCases.Steps.Commands.UpdateRecipeSteps;

public class UpdateRecipeStepsCommandValidator : AbstractValidator<UpdateRecipeStepsCommand>
{
    public UpdateRecipeStepsCommandValidator()
    {
        RuleFor( s => s.Recipe )
            .NotNull().WithMessage( "Рецепт обязателен." );

        RuleFor( s => s.Steps )
            .NotEmpty().WithMessage( "Шаги по приготовлению обязательны." );
    }
}
