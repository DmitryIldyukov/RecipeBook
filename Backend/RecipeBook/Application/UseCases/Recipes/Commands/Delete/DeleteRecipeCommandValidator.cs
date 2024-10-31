using FluentValidation;

namespace Application.UseCases.Recipes.Commands.Delete;

public class DeleteRecipeCommandValidator : AbstractValidator<DeleteRecipeCommand>
{
    public DeleteRecipeCommandValidator()
    {
        RuleFor( s => s.RecipeId )
            .NotNull().WithMessage( "Id рецепта обязателен." )
            .GreaterThan( 0 ).WithMessage( "Идентификатор должен быть положительным числом." );
    }
}
