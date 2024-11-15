using Application.Common.CQRS.Query;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using Application.UseCases.Recipes.Dtos;
using Application.UseCases.Recipes.Queries.GetFavoriteRecipes;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Recipes.Queries.GetUserRecipes;

public class GetUserRecipesQueryHandler(
    IRecipeRepository recipeRepository,
    IValidator<GetUserRecipesQuery> validator,
    IMapper mapper
) : IQueryHandler<GetUserRecipesQuery, ResultT<IReadOnlyList<GetRecipeQueryDto>>>
{
    public async Task<ResultT<IReadOnlyList<GetRecipeQueryDto>>> Handle( GetUserRecipesQuery query )
    {
        ResultT<IReadOnlyList<GetRecipeQueryDto>> validationResult = await ValidateCommandAsync( query );
        if ( !validationResult.IsSuccess )
        {
            return validationResult;
        }

        IReadOnlyList<Recipe> userRecipes = await recipeRepository.GetUserRecipes( query.UserId );

        IReadOnlyList<GetRecipeQueryDto> response = userRecipes.Select( recipe =>
        {
            bool isLiked = recipe.Likes.Any( l => l.UserId == query.UserId );
            bool isFavorite = recipe.Favorites.Any( f => f.UserId == query.UserId );
            GetRecipeQueryDto dto = mapper.Map<GetRecipeQueryDto>( recipe ) with { IsLiked = isLiked, IsFavorite = isFavorite };
            return dto;
        } ).ToList();

        return ResultT<IReadOnlyList<GetRecipeQueryDto>>.Success( response, "Рецепты пользователя получены." );
    }

    private async Task<ResultT<IReadOnlyList<GetRecipeQueryDto>>> ValidateCommandAsync( GetUserRecipesQuery query )
    {
        ValidationResult validationResult = await validator.ValidateAsync( query );
        if ( !validationResult.IsValid )
        {
            return ResultT<IReadOnlyList<GetRecipeQueryDto>>.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        return ResultT<IReadOnlyList<GetRecipeQueryDto>>.Success( null );
    }
}
