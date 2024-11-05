using Application.Common.CQRS.Query;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using Application.UseCases.Tags.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.UseCases.Tags.Queries.GetAll;

public class GetAllTagsQueryHandler(
    ITagRepository tagRepository,
    IMapper mapper
) : IQueryHandler<GetAllTagsQuery, ResultT<IReadOnlyList<GetTagDto>>>
{
    public async Task<ResultT<IReadOnlyList<GetTagDto>>> Handle( GetAllTagsQuery query )
    {
        IReadOnlyList<Tag> tags = await tagRepository.GetAll();

        return ResultT<IReadOnlyList<GetTagDto>>.Success( mapper.Map<IReadOnlyList<GetTagDto>>( tags ), "Тэги успешно получены." );
    }
}
