using Application.Common.Page;

namespace WebAPI.Dtos.Recipe;

public class RecipesByFilterDto
{
    public string SearchString { get; init; }
    public Page Page { get; init; }
}
