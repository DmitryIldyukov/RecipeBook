using Application.Common.CQRS.Query;
using Application.UseCases.Queries.Tags.Dtos;
using Application.UseCases.Queries.Tags.GetAll;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route( "api/[controller]" )]
public class TagController(
    IQueryHandler<GetAllTagsQuery, IReadOnlyList<GetTagDto>> handler
) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType( StatusCodes.Status200OK )]
    public async Task<IActionResult> GetAll()
    {
        GetAllTagsQuery query = new();

        IReadOnlyList<GetTagDto> tags = await handler.Handle( query );
        return Ok( tags );
    }
}
