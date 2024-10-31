using FluentValidation;

namespace Application.UseCases.Ingredients.Commands.Create;

public class CreateIngredientCommandValidator : AbstractValidator<CreateIngredientCommand>
{
    public CreateIngredientCommandValidator()
    {
        RuleFor( i => i.RecipeId )
            .NotNull().WithMessage( "Id рецепта обязателен." );

        RuleFor( i => i.Title )
            .NotEmpty().WithMessage( "Заголовок обязателен." )
            .MaximumLength( 40 ).WithMessage( "Заголовок не может превышать 40 символов." );

        RuleFor( i => i.Description )
            .NotEmpty().WithMessage( "Описание обязательно." );
    }
}
