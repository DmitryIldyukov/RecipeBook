using Application.UseCases.Commands.Dtos.Tags;
using Application.UseCases.Commands.Tags.Create;
using Application.UseCases.Queries.Tags.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper;

public class TagProfile : Profile
{
    public TagProfile()
    {
        CreateMap<Tag, GetTagDto>();
        CreateMap<TagDto, CreateTagCommand>();
        CreateMap<TagDto, Tag>();
        CreateMap<CreateTagCommand, Tag>();
    }
}
