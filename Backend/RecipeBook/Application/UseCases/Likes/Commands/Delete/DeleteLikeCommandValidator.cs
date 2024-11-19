using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.UseCases.Likes.Commands.Delete;

public class DeleteLikeCommandValidator : AbstractValidator<DeleteLikeCommand>
{
    private readonly ILikeRepository _likeRepository;

    public DeleteLikeCommandValidator( ILikeRepository likeRepository )
    {
        _likeRepository = likeRepository;

        RuleFor( l => l.RecipeId )
            .NotEmpty().WithMessage( "Идентификатор рецепта обязателен." );

        RuleFor( l => l.UserId )
            .NotEmpty().WithMessage( "Идентификатор пользователя обязателен." );

        RuleFor( l => l )
           .MustAsync( LikeExists ).WithMessage( "Понравившийся рецепт не найден." );
    }

    private async Task<bool> LikeExists( DeleteLikeCommand command, CancellationToken cancellationToken )
    {
        return await _likeRepository.IsRecipeLikedByUser( command.UserId, command.RecipeId );
    }
}
