using Application.UseCases.Commands.Recipes.Create;
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
        CreateMap<TagDto, Application.UseCases.Commands.Dtos.Tags.TagDto>();
        CreateMap<StepDto, Application.UseCases.Commands.Dtos.Steps.StepDto>();
        CreateMap<IngredientDto, Application.UseCases.Commands.Dtos.Ingredients.IngredientDto>();
    }
}
