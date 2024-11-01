namespace Application.UseCases.Recipes.Commands.Delete;

public record DeleteRecipeCommand
{
    public int RecipeId { get; init; }
}
