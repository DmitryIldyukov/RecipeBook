using Application.Common.Page;

namespace Application.UseCases.Recipes.Queries.GetRecipesByFilter;

public class GetRecipesByFilterQuery
{
    public string SearchString { get; init; }
    public Page Page { get; init; }
}
