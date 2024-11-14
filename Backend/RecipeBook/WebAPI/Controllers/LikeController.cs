using Application.Common.CQRS.Command;
using Application.Common.Result;
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
    ICommandHandler<DeleteLikeCommand, Result> deleteLikeCommand
) : ControllerBase
{
    [HttpPost( "{userId:int}/{recipeId:int}" )]
    [ProducesResponseType( StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> CreateLike( [FromRoute] int userId, [FromRoute] int recipeId )
    {
        CreateLikeCommand command = new CreateLikeCommand()
        {
            UserId = userId,
            RecipeId = recipeId
        };

        Result result = await createLikeCommand.Handle( command );

        if ( result.IsSuccess )
        {
            return Ok();
        }

        return BadRequest( result.ErrorMessages );
    }

    [HttpDelete( "{userId:int}/{recipeId:int}" )]
    [ProducesResponseType( StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> DeleteLike( [FromRoute] int userId, [FromRoute] int recipeId )
    {
        DeleteLikeCommand command = new DeleteLikeCommand()
        {
            UserId = userId,
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
