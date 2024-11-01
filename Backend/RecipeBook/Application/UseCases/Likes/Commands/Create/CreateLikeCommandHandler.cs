using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using Application.Interfaces;
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
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return Result.Failure( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        bool recipeIsExists = await recipeRepository.ContainsAsync( r => r.Id == command.RecipeId );
        if ( !recipeIsExists )
        {
            return Result.Failure( $"Рецепт с Id {command.RecipeId} не найден." );
        }

        bool userHasRecipeInLikes = await likeRepository.UserHasRecipeInLikes( command.UserId, command.RecipeId );
        if ( userHasRecipeInLikes )
        {
            return Result.Failure( "Этот рецепт уже добавлен в понравившееся." );
        }

        Like like = mapper.Map<Like>( command );

        await likeRepository.Create( like );
        await unitOfWork.Commit();

        return Result.Success();
    }
}
