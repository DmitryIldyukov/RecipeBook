using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.Interfaces.Services;
using Application.UseCases.Likes.Commands.Create;
using Application.UseCases.Likes.Commands.Delete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route( "api/[controller]" )]
public class LikeController(
    ICommandHandler<CreateLikeCommand, Result> createLikeCommand,
    ICommandHandler<DeleteLikeCommand, Result> deleteLikeCommand,
    IUserContextService userContextService
) : ControllerBase
{
    [HttpPost( "{recipeId:int}" )]
    [ProducesResponseType( StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> CreateLike( [FromRoute] int recipeId )
    {
        int? userId = userContextService.GetCurrentUserId();

        if ( userId is null )
        {
            return BadRequest( "Id пользователя не найдено." );
        }

        CreateLikeCommand command = new CreateLikeCommand()
        {
            UserId = userId.Value,
            RecipeId = recipeId
        };

        Result result = await createLikeCommand.Handle( command );

        if ( result.IsSuccess )
        {
            return Ok();
        }

        return BadRequest( result.ErrorMessages );
    }

    [HttpDelete( "{recipeId:int}" )]
    [ProducesResponseType( StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> DeleteLike( [FromRoute] int recipeId )
    {
        int? userId = userContextService.GetCurrentUserId();

        if ( userId is null )
        {
            return BadRequest( "Id пользователя не найдено." );
        }

        DeleteLikeCommand command = new DeleteLikeCommand()
        {
            UserId = userId.Value,
            RecipeId = recipeId
        };

        Result result = await deleteLikeCommand.Handle( command );

        if ( result.IsSuccess )
        {
            return Ok();
        }

        return BadRequest( result.ErrorMessages );
    }
}
