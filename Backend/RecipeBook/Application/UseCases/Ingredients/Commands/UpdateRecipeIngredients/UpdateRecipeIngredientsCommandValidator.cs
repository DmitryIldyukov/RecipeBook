using FluentValidation;

namespace Application.UseCases.Ingredients.Commands.UpdateRecipeIngredients;

public class UpdateRecipeIngredientsCommandValidator : AbstractValidator<UpdateRecipeIngredientsCommand>
{
    public UpdateRecipeIngredientsCommandValidator()
    {
        RuleFor( i => i.Recipe )
            .NotNull().WithMessage( "Рецепт обязателен." );

        RuleFor( i => i.Ingredients )
            .NotEmpty().WithMessage( "Ингредиенты обязательны." );
    }
}
