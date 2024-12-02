using Application.Common.Page;

namespace Application.UseCases.Recipes.Queries.GetFavoriteRecipes;

public record GetUserFavoriteRecipesQuery : IPageableQuery
{
    public int UserId { get; init; }
    public Page Page { get; init; }
}
