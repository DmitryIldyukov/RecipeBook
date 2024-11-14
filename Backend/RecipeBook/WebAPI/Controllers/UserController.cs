using Application.Common.CQRS.Command;
using Application.Common.CQRS.Query;
using Application.Common.Result;
using Application.UseCases.RefreshTokens.Commands.Refresh;
using Application.UseCases.Users.Commands.Create;
using Application.UseCases.Users.Commands.Login;
using Application.UseCases.Users.Commands.Update;
using Application.UseCases.Users.Dtos;
using Application.UseCases.Users.Queries.GetById;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos.User;

namespace WebAPI.Controllers;

[ApiController]
[Route( "api/[controller]" )]
public class UserController(
    ICommandHandler<CreateUserCommand, Result> createUserHandler,
    ICommandHandler<UpdateUserCommand, Result> updateUserHandler,
    ICommandHandler<LoginUserCommand, ResultT<TokenInfoDto>> loginUserHandler,
    ICommandHandler<RefreshTokenCommand, ResultT<TokenInfoDto>> refreshTokenHandler,
    IQueryHandler<GetUserByIdQuery, ResultT<GetUserQueryDto>> getUserByIdHandler,
    IMapper mapper
) : ControllerBase
{
    [HttpPost( "Registration" )]
    [ProducesResponseType( StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Register( [FromBody] UserRegisterDto dto )
    {
        CreateUserCommand command = mapper.Map<CreateUserCommand>( dto );
        Result result = await createUserHandler.Handle( command );

        if ( result.IsSuccess )
        {
            return Ok();
        }

        return BadRequest( result.ErrorMessages );
    }

    [Authorize]
    [HttpGet( "{userId:int}" )]
    [ProducesResponseType( typeof( GetUserQueryDto ), StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> GetUserById( [FromRoute] int userId )
    {
        GetUserByIdQuery query = new GetUserByIdQuery()
        {
            Id = userId
        };
        ResultT<GetUserQueryDto> result = await getUserByIdHandler.Handle( query );

        if ( result.IsSuccess )
        {
            return Ok( result.Value );
        }

        return BadRequest( result.ErrorMessages );
    }

    [Authorize]
    [HttpPut( "{userId:int}" )]
    [ProducesResponseType( StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> EditUser( [FromRoute] int userId, [FromBody] UserEditDto dto )
    {
        UpdateUserCommand command = new()
        {
            UserId = userId,
            Name = dto.Name,
            Login = dto.Login,
            Password = dto.Password,
            Information = dto.Information
        };
        Result result = await updateUserHandler.Handle( command );

        if ( result.IsSuccess )
        {
            return Ok();
        }

        return BadRequest( result.ErrorMessages );
    }

    [HttpPost( "Login" )]
    [ProducesResponseType( typeof( TokenInfoDto ), StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Login( [FromBody] LoginDto dto )
    {
        LoginUserCommand command = mapper.Map<LoginUserCommand>( dto );

        ResultT<TokenInfoDto> result = await loginUserHandler.Handle( command );

        if ( result.IsSuccess )
        {
            Response.Cookies.Append( "refresh-token", result.Value.RefreshToken );

            return Ok( result.Value );
        }

        return BadRequest( result.ErrorMessages );
    }

    [HttpGet( "Refresh" )]
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
