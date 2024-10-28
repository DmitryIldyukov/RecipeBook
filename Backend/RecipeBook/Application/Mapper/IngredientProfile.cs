using Application.UseCases.Ingredients.Commands.Create;
using Application.UseCases.Recipes.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper;

public class IngredientProfile : Profile
{
    public IngredientProfile()
    {
        CreateMap<RecipeIngredientDto, CreateIngredientCommand>();
        CreateMap<RecipeIngredientDto, Ingredient>();
        CreateMap<CreateIngredientCommand, Ingredient>();
    }
}
