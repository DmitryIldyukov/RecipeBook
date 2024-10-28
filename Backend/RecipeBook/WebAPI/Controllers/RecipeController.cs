using Application.Common.CQRS.Command;
using Application.Common.CQRS.Query;
using Application.UseCases.Commands.Recipes.Create;
using Application.UseCases.Queries.Recipes.Dtos;
using Application.UseCases.Queries.Recipes.GetRecipeImage;
using AutoMapper;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos.Recipe;

namespace WebAPI.Controllers;

[ApiController]
[Route( "api/[controller]" )]
public class RecipeController(
    ICommandHandler<CreateRecipeCommand> createRecipeHandler,
    IQueryHandler<GetRecipeImageQuery, GetImageQueryDto> getImageHandler,
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
            await createRecipeHandler.Handle( command );

            return Ok();
        }
        catch ( FluentValidation.ValidationException e )
        {
            return BadRequest( e.Errors.Select( error => error.ErrorMessage ).ToList() );
        }
    }

    [HttpGet( "RecipeImage/{recipeId:int}" )]
    [ProducesResponseType( typeof( FileResult ), StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    [ProducesResponseType( typeof( string ), StatusCodes.Status404NotFound )]
    public async Task<IActionResult> GetRecipeImage( [FromRoute] int recipeId )
    {
        GetRecipeImageQuery query = new GetRecipeImageQuery()
        {
            RecipeId = recipeId
        };

        try
        {
            GetImageQueryDto imageDto = await getImageHandler.Handle( query );

            return File( imageDto.File, imageDto.MimeType, imageDto.FileName );
        }
        catch ( FluentValidation.ValidationException e )
        {
            return BadRequest( e.Errors.Select( error => error.ErrorMessage ).ToList() );
        }
        catch ( Exception e ) when ( e is FileNotFoundException || e is NotFoundException )
        {
            return NotFound( e.Message );
        }
    }
}
