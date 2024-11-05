using FluentValidation;

namespace Application.UseCases.Favorites.Commands.Create;

public class CreateFavoriteCommandValidator : AbstractValidator<CreateFavoriteCommand>
{
    public CreateFavoriteCommandValidator()
    {
        RuleFor( f => f.RecipeId )
            .NotEmpty().WithMessage( "Идентификатор рецепта обязателен." );

        RuleFor( f => f.UserId )
            .NotEmpty().WithMessage( "Идентификатор пользователя обязателен." );
    }
}
