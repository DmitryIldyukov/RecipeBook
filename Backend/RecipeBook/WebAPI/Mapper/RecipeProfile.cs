using Application.Common.Page;
using Application.UseCases.Recipes.Commands.Create;
using Application.UseCases.Recipes.Commands.Update;
using Application.UseCases.Recipes.Dtos;
using Application.UseCases.Recipes.Queries.GetFavoriteRecipes;
using Application.UseCases.Recipes.Queries.GetRecipesByFilter;
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
        CreateMap<UpdateRecipeDto, UpdateRecipeCommand>();
        CreateMap<TagDto, RecipeTagDto>();
        CreateMap<StepDto, RecipeStepDto>();
        CreateMap<IngredientDto, RecipeIngredientDto>();
        CreateMap<UpdateTagDto, RecipeTagDto>();
        CreateMap<UpdateStepDto, RecipeStepDto>();
        CreateMap<UpdateIngredientDto, RecipeIngredientDto>();
    }
}
