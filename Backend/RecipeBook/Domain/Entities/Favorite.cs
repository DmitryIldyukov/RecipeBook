namespace Domain.Entities;

public class Favorite : Entity
{
    public int UserId { get; init; }
    public User User { get; init; }
    public int RecipeId { get; init; }
    public Recipe Recipe { get; init; }

    public Favorite( int userId, int recipeId )
    {
        UserId = userId;
        RecipeId = recipeId;
    }
}
