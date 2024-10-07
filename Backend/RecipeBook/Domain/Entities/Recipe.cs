namespace Domain.Entities;

public class Recipe
{
    public int RecipeId { get; init; }
    public int UserId { get; set; }
    public User User { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int CookTime { get; set; }
    public int PortionCount { get; set; }
    public string ImageName { get; set; }

    public ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();
    public ICollection<Step> Steps { get; set; } = new HashSet<Step>();
    public ICollection<Ingredient> Ingredients { get; set; } = new HashSet<Ingredient>();
    public ICollection<Like> Likes { get; set; } = new HashSet<Like>();
    public ICollection<Favorite> Favorites { get; set; } = new HashSet<Favorite>();

    public Recipe(int userId, string name, string description, int cookTime, int portionCount, string imageName )
    {
        Name = name;
        Description = description;
        CookTime = cookTime;
        PortionCount = portionCount;
        ImageName = imageName;
    }
}
