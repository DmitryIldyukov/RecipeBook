namespace Domain.Entities;

public class Step : Entity
{
    public Recipe Recipe { get; init; }
    public string Description { get; set; }

    public Step( Recipe recipe, string description )
    {
        Recipe = recipe;
        Description = description;
    }

    public Step() { }
}
