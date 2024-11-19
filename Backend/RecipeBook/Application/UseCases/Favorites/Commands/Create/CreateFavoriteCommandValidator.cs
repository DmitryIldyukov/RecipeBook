using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.UseCases.Favorites.Commands.Create;

public class CreateFavoriteCommandValidator : AbstractValidator<CreateFavoriteCommand>
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IFavoriteRepository _favoriteRepository;
    private readonly IUserRepository _userRepository;

    public CreateFavoriteCommandValidator(
        IRecipeRepository recipeRepository,
        IFavoriteRepository favoriteRepository,
        IUserRepository userRepository )
    {
        _recipeRepository = recipeRepository;
        _favoriteRepository = favoriteRepository;
        _userRepository = userRepository;

        RuleFor( f => f.RecipeId )
            .NotEmpty().WithMessage( "Идентификатор рецепта обязателен." )
            .MustAsync( RecipeExists ).WithMessage( f => $"Рецепт с Id {f.RecipeId} не найден." );

        RuleFor( f => f.UserId )
            .NotEmpty().WithMessage( "Идентификатор пользователя обязаелен." )
            .MustAsync( UserExists ).WithMessage( u => $"Пользователь с Id {u.UserId} не найден." );

        RuleFor( f => f )
            .MustAsync( UserDoesNotHaveRecipeInFavorites )
            .WithMessage( "Этот рецепт уже добавлен в избранное." );
    }

    private async Task<bool> RecipeExists( int recipeId, CancellationToken cancellationToken )
    {
        return await _recipeRepository.ContainsAsync( r => r.Id == recipeId );
    }

    private async Task<bool> UserExists( int userId, CancellationToken cancellationToken )
    {
        return await _userRepository.ContainsAsync( u => u.Id == userId );
    }

    private async Task<bool> UserDoesNotHaveRecipeInFavorites(
        CreateFavoriteCommand command,
        CancellationToken cancellationToken )
    {
        return !await _favoriteRepository.IsUserFavoriteRecipe( command.UserId, command.RecipeId );
    }
}
