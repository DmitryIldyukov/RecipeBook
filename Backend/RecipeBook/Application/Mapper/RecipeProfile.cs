using Application.UseCases.Recipes.Commands.Create;
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
