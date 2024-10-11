namespace Domain.Entities;

public class Favorite : Entity
{
    public User User { get; init; }
    public Recipe Recipe { get; init; }

    public Favorite( User user, Recipe recipe )
    {
        User = user;
        Recipe = recipe;
    }

    public Favorite() { }
}
