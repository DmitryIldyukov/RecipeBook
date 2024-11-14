using Application.Common.CQRS.Command;
using Application.Common.Result;
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
    ICommandHandler<DeleteFavoriteCommand, Result> deleteFavoriteCommand
) : ControllerBase
{
    [HttpPost( "{userId:int}/{recipeId:int}" )]
    [ProducesResponseType( StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> CreateFavorite( [FromRoute] int userId, [FromRoute] int recipeId )
    {
        CreateFavoriteCommand command = new CreateFavoriteCommand()
        {
            UserId = userId,
            RecipeId = recipeId
        };

        Result result = await createFavoriteCommand.Handle( command );

        if ( result.IsSuccess )
        {
            return Ok();
        }

        return BadRequest( result.ErrorMessages );
    }

    [HttpDelete( "{userId:int}/{recipeId:int}" )]
    [ProducesResponseType( StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> DeleteFavorite( [FromRoute] int userId, [FromRoute] int recipeId )
    {
        DeleteFavoriteCommand command = new DeleteFavoriteCommand()
        {
            UserId = userId,
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
