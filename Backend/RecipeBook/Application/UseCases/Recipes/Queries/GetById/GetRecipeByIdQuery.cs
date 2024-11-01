namespace Application.UseCases.Recipes.Queries.GetById;

public record GetRecipeByIdQuery
{
    public int RecipeId { get; init; }
}
