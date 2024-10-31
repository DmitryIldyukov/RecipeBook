using Application.UseCases.Recipes.Commands.Create;
using Application.UseCases.Recipes.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper;

public class RecipeProfile : Profile
{
    public RecipeProfile()
    {
        CreateMap<CreateRecipeCommand, Recipe>();
        CreateMap<Recipe, DailyRecipeDto>()
            .ForMember( dest => dest.RecipeId, opt => opt.MapFrom( src => src.Id ) )
            .ForMember( dest => dest.LikesCount, opt => opt.MapFrom( src => src.Likes.Count() ) );
    }
}
