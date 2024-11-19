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

public class UserController(
    ICommandHandler<CreateUserCommand, Result> createUserHandler,
    ICommandHandler<UpdateUserCommand, Result> updateUserHandler,
    ICommandHandler<LoginUserCommand, ResultT<TokenInfoDto>> loginUserHandler,
    ICommandHandler<RefreshTokenCommand, ResultT<TokenInfoDto>> refreshTokenHandler,
    IQueryHandler<GetUserByIdQuery, ResultT<GetUserQueryDto>> getUserByIdHandler,
    IMapper mapper
) : BaseController
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
    [HttpGet]
    [ProducesResponseType( typeof( GetUserQueryDto ), StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> GetCurrentUser()
    {
        if ( UserId is null )
        {
            return BadRequest( "Пользователь не найден." );
        }

        GetUserByIdQuery query = new GetUserByIdQuery()
        {
            Id = UserId.Value
        };
        ResultT<GetUserQueryDto> result = await getUserByIdHandler.Handle( query );

        if ( result.IsSuccess )
        {
            return Ok( result.Value );
        }

        return BadRequest( result.ErrorMessages );
    }

    [Authorize]
    [HttpPut]
    [ProducesResponseType( StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> EditUser( [FromBody] UserEditDto dto )
    {
        if ( UserId is null )
        {
            return BadRequest( "Пользователь не найден." );
        }

        UpdateUserCommand command = new()
        {
            UserId = UserId.Value,
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
