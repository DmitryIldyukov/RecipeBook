namespace Domain.Entities;

public class Like : Entity
{
    public int UserId { get; init; }
    public User User { get; init; }
    public int RecipeId { get; init; }
    public Recipe Recipe { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public Like( int userId, int recipeId )
    {
        UserId = userId;
        RecipeId = recipeId;
    }
}
