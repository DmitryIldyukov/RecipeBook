using FluentValidation;

namespace Application.UseCases.Ingredients.Commands.Create;

public class CreateIngredientCommandValidator : AbstractValidator<CreateIngredientCommand>
{
    public CreateIngredientCommandValidator()
    {
        RuleFor( i => i.Recipe )
            .NotNull().WithMessage( "Рецепт обязателен." );

        RuleFor( i => i.Title )
            .NotEmpty().WithMessage( "Заголовок ингредиента обязателен." )
            .MaximumLength( 40 ).WithMessage( "Заголовок ингредиента не может превышать 40 символов." );

        RuleFor( i => i.Description )
            .NotEmpty().WithMessage( "Описание ингредиента обязательно." );
    }
}
