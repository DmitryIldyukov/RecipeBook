using Application.Common.CQRS.Command;
using Application.Common.CQRS.Query;
using Application.Common.Result;
using Application.UseCases.Recipes.Commands.Create;
using Application.UseCases.Recipes.Commands.Delete;
using Application.UseCases.Recipes.Commands.Update;
using Application.UseCases.Recipes.Dtos;
using Application.UseCases.Recipes.Queries.GetById;
using Application.UseCases.Recipes.Queries.GetDailyRecipe;
using Application.UseCases.Recipes.Queries.GetFavoriteRecipes;
using Application.UseCases.Recipes.Queries.GetRecipeImage;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos.Recipe;

namespace WebAPI.Controllers;

[ApiController]
[Route( "api/[controller]" )]
public class RecipeController(
    ICommandHandler<CreateRecipeCommand, Result> createRecipeHandler,
    ICommandHandler<UpdateRecipeCommand, Result> updateRecipeHandler,
    ICommandHandler<DeleteRecipeCommand, Result> deleteRecipeHandler,
    IQueryHandler<GetRecipeImageQuery, ResultT<GetImageQueryDto>> getImageHandler,
    IQueryHandler<GetDailyRecipeQuery, ResultT<DailyRecipeDto>> getDailyRecipeHandler,
    IQueryHandler<GetUserFavoriteRecipesQuery, ResultT<IReadOnlyList<GetRecipeQueryDto>>> getFavoriteRecipesHandler,
    IQueryHandler<GetRecipeByIdQuery, ResultT<GetRecipeQueryDto>> getByIdHandler,
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

    [HttpGet( "DailyRecipe" )]
    [ProducesResponseType( typeof( DailyRecipeDto ), StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> GetDailyRecipe()
    {
        GetDailyRecipeQuery query = new GetDailyRecipeQuery();

        ResultT<DailyRecipeDto> result = await getDailyRecipeHandler.Handle( query );

        if ( result.IsSuccess )
        {
            return Ok( result.Value );
        }

        return BadRequest( result.ErrorMessages );
    }

    [HttpPost( "FavoriteUsers/{userId:int}" )]
    [ProducesResponseType( typeof( IReadOnlyList<GetRecipeQueryDto> ), StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> GetUserFavoritesRecipes( [FromRoute] int userId, [FromBody] FavoriteRecipesDto recipesDto )
    {
        GetUserFavoriteRecipesQuery query = mapper.Map<GetUserFavoriteRecipesQuery>( recipesDto ) with { UserId = userId };

        ResultT<IReadOnlyList<GetRecipeQueryDto>> result = await getFavoriteRecipesHandler.Handle( query );

        if ( result.IsSuccess )
        {
            return Ok( result.Value );
        }

        return BadRequest( result.ErrorMessages );
    }

    [HttpGet( "{recipeId:int}" )]
    [ProducesResponseType( typeof( IReadOnlyList<GetRecipeQueryDto> ), StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> GetRecipeById( [FromRoute] int recipeId )
    {
        GetRecipeByIdQuery query = new GetRecipeByIdQuery()
        {
            RecipeId = recipeId
        };

        ResultT<GetRecipeQueryDto> result = await getByIdHandler.Handle( query );

        if ( result.IsSuccess )
        {
            return Ok( result.Value );
        }

        return BadRequest( result.ErrorMessages );
    }

    [HttpPut( "{recipeId:int}" )]
    [ProducesResponseType( StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> UpdateRecipe( [FromRoute] int recipeId, [FromForm] UpdateRecipeDto dto )
    {
        UpdateRecipeCommand command = mapper.Map<UpdateRecipeCommand>( dto ) with { RecipeId = recipeId };

        Result result = await updateRecipeHandler.Handle( command );
        if ( result.IsSuccess )
        {
            return Ok();
        }

        return BadRequest( result.ErrorMessages );
    }

    [HttpDelete( "{recipeId:int}" )]
    [ProducesResponseType( StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> DeleteRecipe( [FromRoute] int recipeId )
    {
        DeleteRecipeCommand command = new DeleteRecipeCommand()
        {
            RecipeId = recipeId
        };

        Result result = await deleteRecipeHandler.Handle( command );
        if ( result.IsSuccess )
        {
            return Ok();
        }

        return BadRequest( result.ErrorMessages );
    }
}
