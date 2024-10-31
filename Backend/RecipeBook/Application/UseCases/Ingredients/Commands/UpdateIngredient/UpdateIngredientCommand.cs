namespace Application.UseCases.Ingredients.Commands.UpdateIngredient;

public class UpdateIngredientCommand
{
    public int IngredientId { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
}
