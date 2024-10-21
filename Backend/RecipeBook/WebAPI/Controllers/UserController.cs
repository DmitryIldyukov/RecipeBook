using Application.Common.CQRS.Command;
using Application.Common.CQRS.Query;
using Application.UseCases.Commands.Users.Create;
using Application.UseCases.Queries.Users.Dtos;
using Application.UseCases.Queries.Users.GetById;
using AutoMapper;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos.User;

namespace WebAPI.Controllers;

[ApiController]
[Route( "api/[controller]" )]
public class UserController(
    ICommandHandler<CreateUserCommand> createCommandHandler,
    IQueryHandler<GetUserByIdQuery, GetUserQueryDto> getByIdQueryHandler,
    IMapper mapper
) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType( StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Register( [FromBody] UserRegisterDto dto )
    {
        CreateUserCommand command = mapper.Map<CreateUserCommand>( dto );

        try
        {
            await createCommandHandler.Handle( command );

            return Ok();
        }
        catch ( FluentValidation.ValidationException e )
        {
            return BadRequest( e.Errors.Select( error => error.ErrorMessage ).ToList() );
        }
    }

    [HttpGet( "{id}" )]
    [ProducesResponseType( typeof( GetUserQueryDto ), StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    [ProducesResponseType( typeof( string ), StatusCodes.Status404NotFound )]
    public async Task<IActionResult> GetUserById( [FromRoute] int id )
    {
        GetUserByIdQuery query = new GetUserByIdQuery() { Id = id };

        try
        {
            GetUserQueryDto response = await getByIdQueryHandler.Handle( query );

            return Ok( response );
        }
        catch ( FluentValidation.ValidationException e )
        {
            return BadRequest( e.Errors.Select( error => error.ErrorMessage ).ToList() );
        }
        catch ( NotFoundException e )
        {
            return NotFound( e.Message );
        }
    }
}
