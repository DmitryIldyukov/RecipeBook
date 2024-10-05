using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route( "api/[controller]" )]
public class RecipeController( IMediator mediator ) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [ProducesResponseType( typeof( string ), StatusCodes.Status200OK )]
    public async Task<IActionResult> GetWelcomeMessage()
    {
        return Ok( "Hello World!" );
    }
}
