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
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return Result.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        Like like = await likeRepository.GetByUserIdAndRecipeId( command.UserId, command.RecipeId );

        likeRepository.Delete( like );
        await unitOfWork.Commit();

        return Result.Success();
    }
}
