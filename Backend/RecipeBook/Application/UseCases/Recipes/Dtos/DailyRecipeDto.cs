namespace Application.UseCases.Recipes.Dtos;

public record DailyRecipeDto
{
    public int RecipeId { get; init; }
    public int AuthorId { get; init; }
    public string Login { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public int CookTime { get; init; }
    public int LikesCount { get; init; }
}
