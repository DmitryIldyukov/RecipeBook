using Application.Common.CQRS.Command;
using Application.UseCases.Commands.Recipes.Create;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos.Recipe;

namespace WebAPI.Controllers;

[ApiController]
[Route( "api/[controller]" )]
public class RecipeController(
    ICommandHandler<CreateRecipeCommand> createHandler,
    IMapper mapper
) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType( StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> AddRecipe( [FromForm] RecipeDto dto )
    {
        CreateRecipeCommand command = mapper.Map<CreateRecipeCommand>( dto );

        try
        {
            await createHandler.Handle( command );

            return Ok();
        }
        catch ( FluentValidation.ValidationException e )
        {
            return BadRequest( e.Errors.Select( error => error.ErrorMessage ).ToList() );
        }
    }
}
