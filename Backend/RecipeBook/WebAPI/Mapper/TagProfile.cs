using Application.UseCases.Tags.Queries.GetPopular;
using AutoMapper;
using WebAPI.Dtos.Tag;

namespace WebAPI.Mapper;

public class TagProfile : Profile
{
    public TagProfile()
    {
        CreateMap<GetPopularTagsDto, GetPopularTagsQuery>();
    }
}
