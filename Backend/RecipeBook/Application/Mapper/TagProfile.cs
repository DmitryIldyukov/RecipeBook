using Application.UseCases.Recipes.Dtos;
using Application.UseCases.Tags.Commands.Create;
using Application.UseCases.Tags.Dtos;
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
