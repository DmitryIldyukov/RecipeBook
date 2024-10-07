namespace Domain.Entities;

public class Favorite
{
    public int FavoriteId { get; init; }
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
