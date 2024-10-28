using Application.UseCases.Commands.Recipes.Create;
using Application.UseCases.Commands.Recipes.Dtos;
using AutoMapper;
using WebAPI.Dtos.Igredient;
using WebAPI.Dtos.Recipe;
using WebAPI.Dtos.Step;
using WebAPI.Dtos.Tag;

namespace WebAPI.Mapper;

public class RecipeProfile : Profile
{
    public RecipeProfile()
    {
        CreateMap<RecipeDto, CreateRecipeCommand>();
        CreateMap<TagDto, RecipeTagDto>();
        CreateMap<StepDto, RecipeStepDto>();
        CreateMap<IngredientDto, RecipeIngredientDto>();
    }
}
