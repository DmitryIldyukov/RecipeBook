using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.UseCases.Favorites.Commands.Create;
using Application.UseCases.Favorites.Commands.Delete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Authorize]
[Route( "api/favorites" )]
public class FavoriteController(
    ICommandHandler<CreateFavoriteCommand, Result> createFavoriteCommand,
    ICommandHandler<DeleteFavoriteCommand, Result> deleteFavoriteCommand
) : BaseController
{
    [HttpPost( "{recipeId:int}" )]
    [ProducesResponseType( StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> CreateFavorite( [FromRoute] int recipeId )
    {
        if ( UserId is null )
        {
            return BadRequest( "Пользователь не найден." );
        }

        CreateFavoriteCommand command = new CreateFavoriteCommand()
        {
            UserId = UserId.Value,
            RecipeId = recipeId
        };

        Result result = await createFavoriteCommand.Handle( command );

        if ( result.IsSuccess )
        {
            return Ok();
        }

        return BadRequest( result.ErrorMessages );
    }

    [HttpDelete( "{recipeId:int}" )]
    [ProducesResponseType( StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> DeleteFavorite( [FromRoute] int recipeId )
    {
        if ( UserId is null )
        {
            return BadRequest( "Пользователь не найден." );
        }

        DeleteFavoriteCommand command = new DeleteFavoriteCommand()
        {
            UserId = UserId.Value,
            RecipeId = recipeId
        };

        Result result = await deleteFavoriteCommand.Handle( command );

        if ( result.IsSuccess )
        {
            return Ok();
        }

        return BadRequest( result.ErrorMessages );
    }
}
