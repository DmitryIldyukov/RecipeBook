using Application.Common.Page;

namespace Application.UseCases.Recipes.Queries.GetRecipesByFilter;

public record GetRecipesByFilterQuery
{
    public string SearchString { get; init; }
    public Page Page { get; init; }
}
