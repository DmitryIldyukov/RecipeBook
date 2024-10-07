namespace Domain.Entities;

public class Step
{
    public int StepId { get; init; }
    public int RecipeId { get; init; }
    public Recipe Recipe { get; init; }
    public string Description { get; set; }

    public Step( int recipeId, string description )
    {
        RecipeId = recipeId;
        Description = description;
    }
}
