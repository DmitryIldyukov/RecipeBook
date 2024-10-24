namespace Application.UseCases.Commands.Ingredients;

public class CreateIngredientCommand
{
    public int RecipeId { get; set; }
    public string Title { get; init; }
    public string Description { get; init; }
}
