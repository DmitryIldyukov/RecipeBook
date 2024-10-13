using Application.Common.CQRS.Command;
using Application.UseCases.Commands.User.Create;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos.User;

namespace WebAPI.Controllers;

[ApiController]
[Route( "api/[controller]" )]
public class UserController( ICommandHandler<CreateUserCommand> commandHandler ) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType( StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Register( [FromBody] UserRegisterDto dto )
    {
        CreateUserCommand command = new()
        {
            Name = dto.Name,
            Login = dto.Login,
            Password = dto.Password
        };

        try
        {
            await commandHandler.Handle( command );

            return Ok();
        }
        catch ( FluentValidation.ValidationException e )
        {
            return BadRequest( e.Errors.Select( error => error.ErrorMessage ).ToList() );
        }
    }
}
