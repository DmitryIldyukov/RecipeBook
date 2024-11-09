using Application.Common.CQRS.Command;
using Application.Common.CQRS.Query;
using Application.Common.Result;
using Application.UseCases.Users.Commands.Create;
using Application.UseCases.Users.Commands.Login;
using Application.UseCases.Users.Commands.Update;
using Application.UseCases.Users.Dtos;
using Application.UseCases.Users.Queries.GetById;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos.User;

namespace WebAPI.Controllers;

[ApiController]
[Route( "api/[controller]" )]
public class UserController(
    ICommandHandler<CreateUserCommand, Result> createUserCommandHandler,
    ICommandHandler<UpdateUserCommand, Result> updateUserCommandHandler,
    ICommandHandler<LoginUserCommand, ResultT<int>> loginUserCommandHandler,
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
        Result result = await createUserCommandHandler.Handle( command );

        if ( result.IsSuccess )
        {
            return Ok();
        }

        return BadRequest( result.ErrorMessages );
    }

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
        Result result = await updateUserCommandHandler.Handle( command );

        if ( result.IsSuccess )
        {
            return Ok();
        }

        return BadRequest( result.ErrorMessages );
    }

    [HttpPost( "Login" )]
    [ProducesResponseType( typeof( int ), StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Login( [FromBody] LoginDto dto )
    {
        LoginUserCommand command = mapper.Map<LoginUserCommand>( dto );

        ResultT<int> result = await loginUserCommandHandler.Handle( command );

        if ( result.IsSuccess )
        {
            return Ok( result.Value );
        }

        return BadRequest( result.ErrorMessages );
    }
}
