using FluentValidation;

namespace Application.UseCases.Ingredients.Commands.UpdateIngredient;

public class UpdateIngredientCommandValidator : AbstractValidator<UpdateIngredientCommand>
{
    public UpdateIngredientCommandValidator()
    {
        RuleFor( i => i.IngredientId )
            .NotNull().WithMessage( "Идентификатор ингредиента обязателен." )
            .GreaterThan( 0 ).WithMessage( "Идентификатор должен быть положительным числом." );

        RuleFor( i => i.Title )
            .NotEmpty().WithMessage( "Заголовок обязателен." )
            .MaximumLength( 40 ).WithMessage( "Заголовок не может превышать 40 символов." );

        RuleFor( i => i.Description )
            .NotEmpty().WithMessage( "Описание обязательно." );
    }
}
