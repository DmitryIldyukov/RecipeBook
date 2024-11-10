using FluentValidation;

namespace Application.UseCases.Favorites.Commands.Delete;

public class DeleteFavoriteCommandValidator : AbstractValidator<DeleteFavoriteCommand>
{
    public DeleteFavoriteCommandValidator()
    {
        RuleFor( f => f.RecipeId )
            .NotEmpty().WithMessage( "Идентификатор рецепта обязателен." );

        RuleFor( f => f.UserId )
            .NotEmpty().WithMessage( "Идентификатор пользователя обязателен." );
    }
}
