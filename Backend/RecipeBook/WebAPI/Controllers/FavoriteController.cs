using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.Interfaces.Services;
using Application.UseCases.Favorites.Commands.Create;
using Application.UseCases.Favorites.Commands.Delete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route( "api/[controller]" )]
public class FavoriteController(
    ICommandHandler<CreateFavoriteCommand, Result> createFavoriteCommand,
    ICommandHandler<DeleteFavoriteCommand, Result> deleteFavoriteCommand,
    IUserContextService userContextService
) : ControllerBase
{
    [HttpPost( "{recipeId:int}" )]
    [ProducesResponseType( StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> CreateFavorite( [FromRoute] int recipeId )
    {
        int? userId = userContextService.GetCurrentUserId();

        if ( userId is null )
        {
            return BadRequest( "Id пользователя не найдено." );
        }

        CreateFavoriteCommand command = new CreateFavoriteCommand()
        {
            UserId = userId.Value,
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
        int? userId = userContextService.GetCurrentUserId();

        if ( userId is null )
        {
            return BadRequest( "Id пользователя не найдено." );
        }

        DeleteFavoriteCommand command = new DeleteFavoriteCommand()
        {
            UserId = userId.Value,
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
