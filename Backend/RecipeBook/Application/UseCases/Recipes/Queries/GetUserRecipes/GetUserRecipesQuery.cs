namespace Application.UseCases.Recipes.Queries.GetUserRecipes;

public record GetUserRecipesQuery
{
    public int UserId { get; init; }
}
