using Application.Common.CQRS.Query;
using Application.Common.Result;
using Application.UseCases.Tags.Dtos;
using Application.UseCases.Tags.Queries.GetAll;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route( "api/[controller]" )]
public class TagController(
    IQueryHandler<GetAllTagsQuery, ResultT<IReadOnlyList<GetTagDto>>> getTagsHandler
) : ControllerBase
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
}
