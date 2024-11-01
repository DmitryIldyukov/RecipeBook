using Application.Common.CQRS.Query;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using Application.UseCases.Recipes.Dtos;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

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
            return ResultT<IReadOnlyList<GetRecipeQueryDto>>.Failure( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        IQueryable<Recipe> recipes = recipeRepository.GetUserFavoriteRecipesByPage( query.UserId, query.Page );

        IReadOnlyList<GetRecipeQueryDto> response = await mapper.ProjectTo<GetRecipeQueryDto>( recipes ).ToListAsync();

        return ResultT<IReadOnlyList<GetRecipeQueryDto>>.Success( response, $"Избранные рецепты пользователя с id {query.UserId} найдены." );
    }
}
