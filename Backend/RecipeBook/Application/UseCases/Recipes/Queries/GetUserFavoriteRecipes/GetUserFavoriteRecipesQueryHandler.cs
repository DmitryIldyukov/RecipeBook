using Application.Common.BaseHandlers;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using FluentValidation;

namespace Application.UseCases.Recipes.Queries.GetFavoriteRecipes;

public class GetUserFavoriteRecipesQueryHandler : BaseGetRecipesByPageQueryHandler<GetUserFavoriteRecipesQuery>
{
    public GetUserFavoriteRecipesQueryHandler(
        IRecipeRepository recipeRepository,
        IValidator<GetUserFavoriteRecipesQuery> validator,
        IMapper mapper ) : base( recipeRepository, validator, mapper )
    { }

    protected override async Task<IReadOnlyList<Recipe>> GetRecipesWithExtendedPage( GetUserFavoriteRecipesQuery query )
    {
        return await recipeRepository.GetUserFavoriteRecipesByPage( query.UserId, query.Page );
    }

    protected override int? GetUserId( GetUserFavoriteRecipesQuery query ) => query.UserId;
}
