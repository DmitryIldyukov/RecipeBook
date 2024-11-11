using Application.Common.CQRS.Query;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using Application.UseCases.Tags.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.UseCases.Tags.Queries.GetPopular;

public class GetPopularTagsQueryHandler(
    ITagRepository tagRepository,
    IMapper mapper
) : IQueryHandler<GetPopularTagsQuery, ResultT<IReadOnlyList<GetTagDto>>>
{
    public async Task<ResultT<IReadOnlyList<GetTagDto>>> Handle( GetPopularTagsQuery query )
    {
        IReadOnlyList<Tag> tags = await tagRepository.GetPopularTags( query.Count );

        return ResultT<IReadOnlyList<GetTagDto>>.Success( mapper.Map<IReadOnlyList<GetTagDto>>( tags ), "Популярные теги успешно получены." );
    }
}
