using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.UseCases.Likes.Commands.Create;

public class CreateLikeCommandValidator : AbstractValidator<CreateLikeCommand>
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly ILikeRepository _likeRepository;
    private readonly IUserRepository _userRepository;

    public CreateLikeCommandValidator(
        IRecipeRepository recipeRepository,
        ILikeRepository likeRepository,
        IUserRepository userRepository )
    {
        _recipeRepository = recipeRepository;
        _likeRepository = likeRepository;
        _userRepository = userRepository;

        RuleFor( l => l.RecipeId )
            .NotEmpty().WithMessage( "Идентификатор рецепта обязателен." )
            .MustAsync( RecipeExists ).WithMessage( f => $"Рецепт с Id {f.RecipeId} не найден." );

        RuleFor( l => l.UserId )
            .NotEmpty().WithMessage( "Идентификатор пользователя обязателен." )
            .MustAsync( UserExists ).WithMessage( u => $"Пользователь с Id {u.UserId} не найден." );

        RuleFor( l => l )
            .MustAsync( UserDoesNotHaveRecipeInLikes )
            .WithMessage( "Этот рецепт уже добавлен в понравившееся." );
    }

    private async Task<bool> RecipeExists( int recipeId, CancellationToken cancellationToken )
    {
        return await _recipeRepository.ContainsAsync( r => r.Id == recipeId );
    }

    private async Task<bool> UserExists( int userId, CancellationToken cancellationToken )
    {
        return await _userRepository.ContainsAsync( u => u.Id == userId );
    }

    private async Task<bool> UserDoesNotHaveRecipeInLikes(
        CreateLikeCommand command,
        CancellationToken cancellationToken )
    {
        return !await _likeRepository.IsRecipeLikedByUser( command.UserId, command.RecipeId );
    }
}
