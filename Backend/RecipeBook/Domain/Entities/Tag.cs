namespace Domain.Entities;

public class Tag
{
    public int TagId { get; init; }
    public string Name { get; init; }

    public ICollection<Recipe> Recipes { get; set; } = new HashSet<Recipe>();

    public Tag( string name )
    {
        Name = name;
    }
}
