using Application.UseCases.Commands.Ingredients.Create;
using Application.UseCases.Commands.Recipes.Dtos;
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
