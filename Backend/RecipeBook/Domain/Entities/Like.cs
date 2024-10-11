namespace Domain.Entities;

public class Like : Entity
{
    public User User { get; init; }
    public Recipe Recipe { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public Like( User user, Recipe recipe )
    {
        User = user;
        Recipe = recipe;
    }

    public Like() { }
}
