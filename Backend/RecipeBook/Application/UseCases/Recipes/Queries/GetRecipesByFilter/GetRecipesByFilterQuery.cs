using Application.Common.Page;

namespace Application.UseCases.Recipes.Queries.GetRecipesByFilter;

public record GetRecipesByFilterQuery
{
    public int? UserId { get; init; }
    public string SearchString { get; init; }
    public Page Page { get; init; }
}
