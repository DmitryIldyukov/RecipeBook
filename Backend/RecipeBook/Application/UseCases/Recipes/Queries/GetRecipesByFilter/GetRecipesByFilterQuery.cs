using Application.Common.Page;

namespace Application.UseCases.Recipes.Queries.GetRecipesByFilter;

public record GetRecipesByFilterQuery : IPageableQuery
{
    public int? UserId { get; init; }
    public List<string> SearchQueries { get; init; }
    public Page Page { get; init; }
}
