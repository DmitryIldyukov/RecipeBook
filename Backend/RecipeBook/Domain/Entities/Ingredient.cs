namespace Domain.Entities;

public class Ingredient : Entity
{
    public int RecipeId { get; init; }
    public Recipe Recipe { get; init; }
    public string Title { get; set; }
    public string Description { get; set; }

    public Ingredient( int recipeId, string title, string description )
    {
        RecipeId = recipeId;
        Title = title;
        Description = description;
    }
}
