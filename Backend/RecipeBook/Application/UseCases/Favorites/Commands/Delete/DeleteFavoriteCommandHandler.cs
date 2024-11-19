using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Favorites.Commands.Delete;

public class DeleteFavoriteCommandHandler(
    IFavoriteRepository favoriteRepository,
    IValidator<DeleteFavoriteCommand> validator,
    IUnitOfWork unitOfWork
) : ICommandHandler<DeleteFavoriteCommand, Result>
{
    public async Task<Result> Handle( DeleteFavoriteCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return Result.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        Favorite favorite = await favoriteRepository.GetByUserIdAndRecipeId( command.UserId, command.RecipeId );

        favoriteRepository.Delete( favorite );
        await unitOfWork.Commit();

        return Result.Success();
    }
}
