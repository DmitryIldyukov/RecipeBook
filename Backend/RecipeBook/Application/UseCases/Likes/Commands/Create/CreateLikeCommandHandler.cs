using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Likes.Commands.Create;

public class CreateLikeCommandHandler(
    ILikeRepository likeRepository,
    IRecipeRepository recipeRepository,
    IValidator<CreateLikeCommand> validator,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ICommandHandler<CreateLikeCommand, Result>
{
    public async Task<Result> Handle( CreateLikeCommand command )
    {
        Result validationResult = await ValidateCommandAsync( command );
        if ( !validationResult.IsSuccess )
        {
            return validationResult;
        }

        bool recipeIsExists = await recipeRepository.ContainsAsync( r => r.Id == command.RecipeId );
        if ( !recipeIsExists )
        {
            return Result.Fail( $"Рецепт с Id {command.RecipeId} не найден." );
        }

        bool userHasRecipeInLikes = await likeRepository.IsRecipeLikedByUser( command.UserId, command.RecipeId );
        if ( userHasRecipeInLikes )
        {
            return Result.Fail( "Этот рецепт уже добавлен в понравившееся." );
        }

        Like like = mapper.Map<Like>( command );

        await likeRepository.Create( like );
        await unitOfWork.Commit();

        return Result.Success();
    }

    private async Task<Result> ValidateCommandAsync( CreateLikeCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return Result.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        return Result.Success();
    }
}
