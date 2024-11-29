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
        CreateMap<FavoriteRecipesDto, GetUserFavoriteRecipesQuery>();
        CreateMap<TagDto, RecipeTagDto>();
        CreateMap<StepDto, RecipeStepDto>();
        CreateMap<IngredientDto, RecipeIngredientDto>();
        CreateMap<UpdateTagDto, RecipeTagDto>();
        CreateMap<UpdateStepDto, RecipeStepDto>();
        CreateMap<UpdateIngredientDto, RecipeIngredientDto>();

        CreateMap<RecipesByFilterDto, Page>()
            .ForMember( dest => dest.PageNumber, opt => opt.MapFrom( src => src.PageNumber ) )
            .ForMember( dest => dest.PageSize, opt => opt.MapFrom( src => src.PageSize ) );

        CreateMap<RecipesByFilterDto, GetRecipesByFilterQuery>()
            .ForMember( dest => dest.Page, opt => opt.MapFrom( src => src ) );
    }
}
