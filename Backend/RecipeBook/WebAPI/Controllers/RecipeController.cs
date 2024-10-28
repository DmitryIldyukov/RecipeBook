using Application.Common.CQRS.Command;
using Application.Common.CQRS.Query;
using Application.Common.Result;
using Application.UseCases.Recipes.Commands.Create;
using Application.UseCases.Recipes.Dtos;
using Application.UseCases.Recipes.Queries.GetRecipeImage;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos.Recipe;

namespace WebAPI.Controllers;

[ApiController]
[Route( "api/[controller]" )]
public class RecipeController(
    ICommandHandler<CreateRecipeCommand, Result> createRecipeHandler,
    IQueryHandler<GetRecipeImageQuery, ResultT<GetImageQueryDto>> getImageHandler,
    IMapper mapper
) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType( StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> AddRecipe( [FromForm] RecipeDto dto )
    {
        CreateRecipeCommand command = mapper.Map<CreateRecipeCommand>( dto );
        Result result = await createRecipeHandler.Handle( command );
        if ( result.IsSuccess )
        {
            return Ok();
        }

        return BadRequest( result.ErrorMessages );
    }

    [HttpGet( "RecipeImage/{recipeId:int}" )]
    [ProducesResponseType( typeof( FileResult ), StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> GetRecipeImage( [FromRoute] int recipeId )
    {
        GetRecipeImageQuery query = new GetRecipeImageQuery()
        {
            RecipeId = recipeId
        };

        ResultT<GetImageQueryDto> result = await getImageHandler.Handle( query );

        if ( result.IsSuccess )
        {
            return File( result.Value.File, result.Value.MimeType, result.Value.FileName );
        }

        return BadRequest( result.ErrorMessages );
    }
}
