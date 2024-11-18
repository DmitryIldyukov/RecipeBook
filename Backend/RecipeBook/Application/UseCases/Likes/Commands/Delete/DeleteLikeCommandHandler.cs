using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Likes.Commands.Delete;

public class DeleteLikeCommandHandler(
    ILikeRepository likeRepository,
    IValidator<DeleteLikeCommand> validator,
    IUnitOfWork unitOfWork
) : ICommandHandler<DeleteLikeCommand, Result>
{
    public async Task<Result> Handle( DeleteLikeCommand command )
    {
        Like like = await likeRepository.GetByUserIdAndRecipeId( command.UserId, command.RecipeId );

        Result validationResult = await ValidateAsync( command, like );
        if ( !validationResult.IsSuccess )
        {
            return validationResult;
        }

        likeRepository.Delete( like );
        await unitOfWork.Commit();

        return Result.Success();
    }

    private async Task<Result> ValidateAsync( DeleteLikeCommand command, Like like )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return Result.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        if ( like is null )
        {
            return Result.Fail( "Понравившийся рецепт не найден." );
        }

        Result validationUserOwnershipResult = ValidateUserOwnership( like.UserId, command.UserId );
        if ( !validationUserOwnershipResult.IsSuccess )
        {
            return validationUserOwnershipResult;
        }

        return Result.Success();
    }

    private Result ValidateUserOwnership( int likeUserId, int currentUserId )
    {
        if ( likeUserId != currentUserId )
        {
            return Result.Fail( "Невозможно удалить рецепт из понравившихся рецептов у другого пользователя." );
        }

        return Result.Success();
    }
}
