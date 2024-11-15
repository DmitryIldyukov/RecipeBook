using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Application.UseCases.Favorites.Commands.Delete;

public class DeleteFavoriteCommandHandler(
    IFavoriteRepository favoriteRepository,
    IValidator<DeleteFavoriteCommand> validator,
    IUnitOfWork unitOfWork
) : ICommandHandler<DeleteFavoriteCommand, Result>
{
    public async Task<Result> Handle( DeleteFavoriteCommand command )
    {
        Result validationResult = await ValidateCommandAsync( command );
        if ( !validationResult.IsSuccess )
        {
            return validationResult;
        }

        Favorite favorite = await favoriteRepository.GetByUserIdAndRecipeId( command.UserId, command.RecipeId );
        if ( favorite is null )
        {
            return Result.Fail( "Избранный рецепт не найден." );
        }

        Result validationUserOwnershipResult = ValidateUserOwnership( favorite.UserId, command.UserId );
        if ( !validationUserOwnershipResult.IsSuccess )
        {
            return validationUserOwnershipResult;
        }

        favoriteRepository.Delete( favorite );
        await unitOfWork.Commit();

        return Result.Success();
    }

    private async Task<Result> ValidateCommandAsync( DeleteFavoriteCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return Result.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        return Result.Success();
    }

    private Result ValidateUserOwnership( int favoriteUserId, int currentUserId )
    {
        if ( favoriteUserId != currentUserId )
        {
            return Result.Fail( "Невозможно удалить рецепт из избранного у другого пользователя." );
        }

        return Result.Success();
    }
}
