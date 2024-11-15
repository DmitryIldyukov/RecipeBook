using System.Collections.Generic;
using Application.Common.CQRS.Query;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using Application.UseCases.Recipes.Dtos;
using Application.UseCases.Recipes.Queries.GetRecipeImage;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Recipes.Queries.GetRecipesByFilter;

public class GetRecipesByFilterQueryHandler(
    IRecipeRepository recipeRepository,
    IValidator<GetRecipesByFilterQuery> validator,
    IMapper mapper
) : IQueryHandler<GetRecipesByFilterQuery, ResultT<IReadOnlyList<GetRecipeQueryDto>>>
{
    public async Task<ResultT<IReadOnlyList<GetRecipeQueryDto>>> Handle( GetRecipesByFilterQuery query )
    {
        ResultT<IReadOnlyList<GetRecipeQueryDto>> validationResult = await ValidateCommandAsync( query );
        if ( !validationResult.IsSuccess )
        {
            return validationResult;
        }

        IReadOnlyList<Recipe> recipes = await recipeRepository.GetRecipesByFilter( query.SearchQueries, query.Page );

        IReadOnlyList<GetRecipeQueryDto> response = recipes.Select( recipe =>
        {
            bool isLiked = recipe.Likes.Any( l => l.UserId == query.UserId );
            bool isFavorite = recipe.Favorites.Any( f => f.UserId == query.UserId );
            GetRecipeQueryDto dto = mapper.Map<GetRecipeQueryDto>( recipe ) with { IsLiked = isLiked, IsFavorite = isFavorite };
            return dto;
        } ).ToList();

        return ResultT<IReadOnlyList<GetRecipeQueryDto>>.Success( response, "Рецепты найдены." );
    }

    private async Task<ResultT<IReadOnlyList<GetRecipeQueryDto>>> ValidateCommandAsync( GetRecipesByFilterQuery query )
    {
        ValidationResult validationResult = await validator.ValidateAsync( query );
        if ( !validationResult.IsValid )
        {
            return ResultT<IReadOnlyList<GetRecipeQueryDto>>.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        return ResultT<IReadOnlyList<GetRecipeQueryDto>>.Success( null );
    }
}
