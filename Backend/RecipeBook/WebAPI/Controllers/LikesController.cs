using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.UseCases.Likes.Commands.Create;
using Application.UseCases.Likes.Commands.Delete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Authorize]
[Route( "api/[controller]" )]
public class LikesController(
    ICommandHandler<CreateLikeCommand, Result> createLikeCommand,
    ICommandHandler<DeleteLikeCommand, Result> deleteLikeCommand
) : BaseController
{
    [HttpPost( "{recipeId:int}" )]
    [ProducesResponseType( StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> CreateLike( [FromRoute] int recipeId )
    {
        if ( UserId is null )
        {
            return BadRequest( "Пользователь не найден." );
        }

        CreateLikeCommand command = new CreateLikeCommand()
        {
            UserId = UserId.Value,
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
        if ( UserId is null )
        {
            return BadRequest( "Пользователь не найден." );
        }

        DeleteLikeCommand command = new DeleteLikeCommand()
        {
            UserId = UserId.Value,
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
