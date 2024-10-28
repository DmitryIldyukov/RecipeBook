using Application.UseCases.Commands.Dtos.Ingredients;
using Application.UseCases.Commands.Ingredients.Create;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper;

public class IngredientProfile : Profile
{
    public IngredientProfile()
    {
        CreateMap<IngredientDto, CreateIngredientCommand>();
        CreateMap<IngredientDto, Ingredient>();
        CreateMap<CreateIngredientCommand, Ingredient>();
    }
}
