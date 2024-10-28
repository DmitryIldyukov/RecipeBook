using Application.UseCases.Commands.Recipes.Dtos;
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
        CreateMap<RecipeTagDto, CreateTagCommand>();
        CreateMap<RecipeTagDto, Tag>();
        CreateMap<CreateTagCommand, Tag>();
    }
}
