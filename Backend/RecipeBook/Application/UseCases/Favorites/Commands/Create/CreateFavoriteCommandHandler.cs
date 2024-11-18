using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Favorites.Commands.Create;

public class CreateFavoriteCommandHandler(
    IFavoriteRepository favoriteRepository,
    IRecipeRepository recipeRepository,
    IValidator<CreateFavoriteCommand> validator,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ICommandHandler<CreateFavoriteCommand, Result>
{
    public async Task<Result> Handle( CreateFavoriteCommand command )
    {
        Result validationResult = await ValidateAsync( command );
        if ( !validationResult.IsSuccess )
        {
            return validationResult;
        }

        Favorite favorite = mapper.Map<Favorite>( command );

        await favoriteRepository.Create( favorite );
        await unitOfWork.Commit();

        return Result.Success();
    }

    private async Task<Result> ValidateAsync( CreateFavoriteCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return Result.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        bool recipeIsExists = await recipeRepository.ContainsAsync( r => r.Id == command.RecipeId );
        if ( !recipeIsExists )
        {
            return Result.Fail( $"Рецепт с Id {command.RecipeId} не найден." );
        }

        bool userHasRecipeInFavorites = await favoriteRepository.IsUserFavoriteRecipe( command.UserId, command.RecipeId );
        if ( userHasRecipeInFavorites )
        {
            return Result.Fail( "Этот рецепт уже добавлен в избранное." );
        }

        return Result.Success();
    }
}
