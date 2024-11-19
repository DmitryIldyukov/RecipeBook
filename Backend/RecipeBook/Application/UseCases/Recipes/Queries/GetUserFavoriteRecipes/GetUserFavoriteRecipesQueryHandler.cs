using Application.Common.CQRS.Query;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using Application.UseCases.Recipes.Dtos;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Recipes.Queries.GetFavoriteRecipes;

public class GetUserFavoriteRecipesQueryHandler(
    IRecipeRepository recipeRepository,
    IValidator<GetUserFavoriteRecipesQuery> validator,
    IMapper mapper
) : IQueryHandler<GetUserFavoriteRecipesQuery, ResultT<IReadOnlyList<GetRecipeQueryDto>>>
{
    public async Task<ResultT<IReadOnlyList<GetRecipeQueryDto>>> Handle( GetUserFavoriteRecipesQuery query )
    {
        ValidationResult validationResult = await validator.ValidateAsync( query );
        if ( !validationResult.IsValid )
        {
            return ResultT<IReadOnlyList<GetRecipeQueryDto>>.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        IReadOnlyList<Recipe> recipes = await recipeRepository.GetUserFavoriteRecipesByPage( query.UserId, query.Page );

        IReadOnlyList<GetRecipeQueryDto> response = recipes.Select( recipe =>
        {
            bool isLiked = recipe.Likes.Any( l => l.UserId == query.UserId );
            bool isFavorite = recipe.Favorites.Any( f => f.UserId == query.UserId );
            GetRecipeQueryDto dto = mapper.Map<GetRecipeQueryDto>( recipe ) with { IsLiked = isLiked, IsFavorite = isFavorite };
            return dto;
        } ).ToList();

        return ResultT<IReadOnlyList<GetRecipeQueryDto>>.Success( response, $"Избранные рецепты пользователя с id {query.UserId} найдены." );
    }
}
