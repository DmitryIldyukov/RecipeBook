namespace WebAPI.Dtos.Recipe;

public class RecipesByFilterDto
{
    public List<string> SearchQueries { get; init; } = new List<string>();
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}
