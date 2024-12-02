using Application.Common.BaseHandlers;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using FluentValidation;

namespace Application.UseCases.Recipes.Queries.GetRecipesByFilter;

public class GetRecipesByFilterQueryHandler : BaseGetRecipesByPageQueryHandler<GetRecipesByFilterQuery>
{
    public GetRecipesByFilterQueryHandler(
        IRecipeRepository recipeRepository,
        IValidator<GetRecipesByFilterQuery> validator,
        IMapper mapper ) : base( recipeRepository, validator, mapper )
    { }

    protected override async Task<IReadOnlyList<Recipe>> GetRecipesWithExtendedPage( GetRecipesByFilterQuery query )
    {
        return await recipeRepository.GetRecipesByFilter( query.SearchQueries, query.Page );
    }

    protected override int? GetUserId( GetRecipesByFilterQuery query ) => query.UserId;
}
