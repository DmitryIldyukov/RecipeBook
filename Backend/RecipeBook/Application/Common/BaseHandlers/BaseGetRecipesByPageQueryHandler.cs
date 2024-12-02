using Application.Common.CQRS.Query;
using Application.Common.Page;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using Application.UseCases.Recipes.Dtos;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.Common.BaseHandlers;

public abstract class BaseGetRecipesByPageQueryHandler<TQuery>
    : IQueryHandler<TQuery, ResultT<GetRecipesByPageDto>>
    where TQuery : class, IPageableQuery
{
    protected readonly IRecipeRepository recipeRepository;
    protected readonly IValidator<TQuery> validator;
    protected readonly IMapper mapper;

    protected BaseGetRecipesByPageQueryHandler(
        IRecipeRepository recipeRepository,
        IValidator<TQuery> validator,
        IMapper mapper )
    {
        this.recipeRepository = recipeRepository;
        this.validator = validator;
        this.mapper = mapper;
    }

    public async Task<ResultT<GetRecipesByPageDto>> Handle( TQuery query )
    {
        ValidationResult validationResult = await validator.ValidateAsync( query );
        if ( !validationResult.IsValid )
        {
            return ResultT<GetRecipesByPageDto>.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        IReadOnlyList<Recipe> recipes = await GetRecipesWithExtendedPage( query );
        bool hasMoreRecipes = HasAdditionalRecipes( recipes, query.Page.PageSize );
        recipes = GetRecipesForCurrentPage( recipes, query.Page.PageSize );

        IReadOnlyList<GetRecipeQueryDto> recipeDtos = MapToRecipeDtos( recipes, GetUserId( query ) );

        GetRecipesByPageDto response = new GetRecipesByPageDto
        {
            HasTakeMoreRecipes = hasMoreRecipes,
            Recipes = recipeDtos
        };

        return ResultT<GetRecipesByPageDto>.Success( response, "Рецепты найдены." );
    }

    protected abstract Task<IReadOnlyList<Recipe>> GetRecipesWithExtendedPage( TQuery query );

    protected virtual int? GetUserId( TQuery query ) => null;

    private bool HasAdditionalRecipes( IReadOnlyList<Recipe> recipes, int pageSize )
    {
        return recipes.Count > pageSize;
    }

    private IReadOnlyList<Recipe> GetRecipesForCurrentPage( IReadOnlyList<Recipe> recipes, int pageSize )
    {
        return recipes.Take( pageSize ).ToList();
    }

    private IReadOnlyList<GetRecipeQueryDto> MapToRecipeDtos( IReadOnlyList<Recipe> recipes, int? userId )
    {
        return recipes.Select( recipe =>
        {
            bool isLiked = recipe.Likes.Any( l => l.UserId == userId );
            bool isFavorite = recipe.Favorites.Any( f => f.UserId == userId );
            return mapper.Map<GetRecipeQueryDto>( recipe ) with
            {
                IsLiked = isLiked,
                IsFavorite = isFavorite
            };
        } ).ToList();
    }
}
