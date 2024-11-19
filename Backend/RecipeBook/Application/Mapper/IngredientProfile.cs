using Application.UseCases.Ingredients.Commands.Create;
using Application.UseCases.Ingredients.Dtos;
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
        CreateMap<CreateIngredientCommand, Ingredient>()
            .ForMember( dest => dest.Id, opt => opt.MapFrom( src => src.Recipe.Id ) );
        CreateMap<Ingredient, GetIngredientDto>();
    }
}
