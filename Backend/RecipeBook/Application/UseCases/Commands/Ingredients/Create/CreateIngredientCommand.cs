namespace Application.UseCases.Commands.Ingredients.Create;

public class CreateIngredientCommand
{
    public int RecipeId { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
}
