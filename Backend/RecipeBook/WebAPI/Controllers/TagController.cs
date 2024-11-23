using Application.Common.CQRS.Query;
using Application.Common.Result;
using Application.UseCases.Tags.Dtos;
using Application.UseCases.Tags.Queries.GetAll;
using Application.UseCases.Tags.Queries.GetPopular;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos.Tag;

namespace WebAPI.Controllers;

[ApiController]
[Route( "api/tags" )]
public class TagController(
    IQueryHandler<GetAllTagsQuery, ResultT<IReadOnlyList<GetTagDto>>> getTagsHandler,
    IQueryHandler<GetPopularTagsQuery, ResultT<IReadOnlyList<GetTagDto>>> getPopularTagsHandler,
    IMapper mapper
) : BaseController
{
    [HttpGet]
    [ProducesResponseType( StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> GetAll()
    {
        GetAllTagsQuery query = new();
        ResultT<IReadOnlyList<GetTagDto>> result = await getTagsHandler.Handle( query );

        if ( result.IsSuccess )
        {
            return Ok( result.Value );
        }

        return BadRequest( result.ErrorMessages );
    }

    [HttpGet( "popular" )]
    [ProducesResponseType( StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> GetPopularTags( [FromQuery] GetPopularTagsDto popualTagsDto )
    {
        GetPopularTagsQuery query = mapper.Map<GetPopularTagsQuery>( popualTagsDto );
        ResultT<IReadOnlyList<GetTagDto>> result = await getPopularTagsHandler.Handle( query );

        if ( result.IsSuccess )
        {
            return Ok( result.Value );
        }

        return BadRequest( result.ErrorMessages );
    }
}
