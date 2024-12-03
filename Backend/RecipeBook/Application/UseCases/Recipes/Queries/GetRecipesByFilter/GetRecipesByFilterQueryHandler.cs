using Application.Common.CQRS.Query;
using Application.Common.Page;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using Application.UseCases.Recipes.Dtos;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Recipes.Queries.GetRecipesByFilter;

public class GetRecipesByFilterQueryHandler(
    IRecipeRepository recipeRepository,
    IValidator<GetRecipesByFilterQuery> validator,
    IMapper mapper
) : IQueryHandler<GetRecipesByFilterQuery, ResultT<GetRecipesByPageDto>>
{
    public async Task<ResultT<GetRecipesByPageDto>> Handle( GetRecipesByFilterQuery query )
    {
        ValidationResult validationResult = await validator.ValidateAsync( query );
        if ( !validationResult.IsValid )
        {
            return ResultT<GetRecipesByPageDto>.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        IReadOnlyList<Recipe> recipes = await recipeRepository.GetRecipesByFilter( query.SearchQueries, query.Page );

        IReadOnlyList<GetRecipeQueryDto> mappedRecipes = recipes.Select( recipe =>
        {
            bool isLiked = recipe.Likes.Any( l => l.UserId == query.UserId );
            bool isFavorite = recipe.Favorites.Any( f => f.UserId == query.UserId );
            GetRecipeQueryDto dto = mapper.Map<GetRecipeQueryDto>( recipe ) with { IsLiked = isLiked, IsFavorite = isFavorite };

            return dto;
        } ).ToList();

        bool hasLoadMoreRecipes = await recipeRepository.AnyRecipesByFilters( query.SearchQueries, new Page { PageNumber = query.Page.PageNumber + 1, PageSize = query.Page.PageSize } );

        GetRecipesByPageDto response = new GetRecipesByPageDto()
        {
            Recipes = mappedRecipes,
            HasTakeMoreRecipes = hasLoadMoreRecipes
        };

        return ResultT<GetRecipesByPageDto>.Success( response, "Рецепты найдены." );
    }
}
