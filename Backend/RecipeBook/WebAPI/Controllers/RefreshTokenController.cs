using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.UseCases.RefreshTokens.Commands.Refresh;
using Application.UseCases.Users.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

public class RefreshTokenController( ICommandHandler<RefreshTokenCommand, ResultT<TokenInfoDto>> refreshTokenHandler ) : BaseController
{
    [HttpGet]
    [ProducesResponseType( typeof( TokenInfoDto ), StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Refresh()
    {
        string requestRefreshToken = Request.Cookies[ "refresh-token" ];
        RefreshTokenCommand command = new RefreshTokenCommand()
        {
            RefreshToken = requestRefreshToken
        };
        ResultT<TokenInfoDto> result = await refreshTokenHandler.Handle( command );

        if ( result.IsSuccess )
        {
            Response.Cookies.Append( "refresh-token", result.Value.RefreshToken );

            return Ok( result.Value );
        }

        return BadRequest( result.ErrorMessages );
    }
}
