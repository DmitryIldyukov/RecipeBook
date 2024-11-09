namespace Application.UseCases.Recipes.Queries.GetById;

public record GetRecipeByIdQuery
{
    public int? UserId { get; init; }
    public int RecipeId { get; init; }
}
