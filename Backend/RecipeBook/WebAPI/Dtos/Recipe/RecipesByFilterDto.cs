using Application.Common.Page;

namespace WebAPI.Dtos.Recipe;

public class RecipesByFilterDto
{
    public List<string> SearchQueries { get; init; }
    public Page Page { get; init; }
}
