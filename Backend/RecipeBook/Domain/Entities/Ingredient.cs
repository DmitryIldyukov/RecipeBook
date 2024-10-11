namespace Domain.Entities;

public class Ingredient : Entity
{
    public Recipe Recipe { get; init; }
    public string Title { get; set; }
    public string Description { get; set; }

    public Ingredient( Recipe recipe, string title, string description )
    {
        Recipe = recipe;
        Title = title;
        Description = description;
    }

    public Ingredient() { }
}
