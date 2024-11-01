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
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return Result.Failure( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        if ( await recipeRepository.GetById( command.RecipeId ) is null )
        {
            return Result.Failure( $"Рецепт с Id {command.RecipeId} не найден." );
        }

        bool userHasRecipeInFavorites = await favoriteRepository.UserHasRecipeInFavorites( command.UserId, command.RecipeId );
        if ( userHasRecipeInFavorites )
        {
            return Result.Failure( "Этот рецепт уже добавлен в избранное." );
        }

        Favorite favorite = mapper.Map<Favorite>( command );

        await favoriteRepository.Create( favorite );
        await unitOfWork.Commit();

        return Result.Success();
    }
}
