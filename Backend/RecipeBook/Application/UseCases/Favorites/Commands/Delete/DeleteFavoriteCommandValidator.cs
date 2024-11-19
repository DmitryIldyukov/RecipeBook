using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.UseCases.Favorites.Commands.Delete;

public class DeleteFavoriteCommandValidator : AbstractValidator<DeleteFavoriteCommand>
{
    private readonly IFavoriteRepository _favoriteRepository;

    public DeleteFavoriteCommandValidator( IFavoriteRepository favoriteRepository )
    {
        _favoriteRepository = favoriteRepository;

        RuleFor( f => f.RecipeId )
            .NotEmpty().WithMessage( "Идентификатор рецепта обязателен." );

        RuleFor( f => f.UserId )
            .NotEmpty().WithMessage( "Идентификатор пользователя обязателен." );

        RuleFor( f => f )
           .MustAsync( FavoriteExists ).WithMessage( "Избранный рецепт не найден." );
    }

    private async Task<bool> FavoriteExists( DeleteFavoriteCommand command, CancellationToken cancellationToken )
    {
        return await _favoriteRepository.IsUserFavoriteRecipe( command.UserId, command.RecipeId );
    }
}
