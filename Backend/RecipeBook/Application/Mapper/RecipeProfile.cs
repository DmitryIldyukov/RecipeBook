using Application.UseCases.Commands.Recipes.Create;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper;

public class RecipeProfile : Profile
{
    public RecipeProfile()
    {
        CreateMap<CreateRecipeCommand, Recipe>();
    }
}
