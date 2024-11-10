using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using Application.Interfaces;
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

        Like like = await likeRepository.GetByUserAndRecipeId( command.UserId, command.RecipeId );

        if ( like is null )
        {
            return Result.Fail( "Понравившийся рецепт не найден." );
        }

        if ( like.UserId != command.UserId )
        {
            return Result.Fail( "Невозможно удалить рецепт из понравившихся рецептов у другого пользователя." );
        }

        likeRepository.Delete( like );
        await unitOfWork.Commit();

        return Result.Success();
    }
}
