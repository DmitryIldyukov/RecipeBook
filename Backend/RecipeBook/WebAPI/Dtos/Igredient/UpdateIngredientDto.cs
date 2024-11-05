namespace WebAPI.Dtos.Igredient;

public class UpdateIngredientDto
{
    public int? IngredientId { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
}
