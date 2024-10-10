namespace Domain.Entities;

public class Tag : Entity
{
    public string Name { get; init; }

    public ICollection<Recipe> Recipes { get; set; } = new HashSet<Recipe>();

    public Tag( string name )
    {
        Name = name;
    }
}
