namespace Domain.Entities;

public class Recipe : Entity
{
    public User Author { get; init; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int CookTime { get; set; }
    public int PortionCount { get; set; }
    public string ImageName { get; set; }

    public ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();
    public ICollection<Step> Steps { get; set; } = new List<Step>();
    public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
    public ICollection<Like> Likes { get; set; } = new HashSet<Like>();
    public ICollection<Favorite> Favorites { get; set; } = new HashSet<Favorite>();

    public Recipe( User author, string name, string description, int cookTime, int portionCount, string imageName)
    {
        Author = author;
        Name = name;
        Description = description;
        CookTime = cookTime;
        PortionCount = portionCount;
        ImageName = imageName;
    }

    public Recipe() { }
}
