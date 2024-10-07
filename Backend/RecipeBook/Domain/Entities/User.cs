namespace Domain.Entities;

public class User
{
    public int UserId { get; init; }
    public string Name { get; set; }
    public string Login { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; }
    public string Information { get; set; } = string.Empty;

    public ICollection<Recipe> Recipes { get; set; } = new HashSet<Recipe>();
    public ICollection<Like> Likes { get; set; } = new HashSet<Like>();
    public ICollection<Favorite> Favorites { get; set; } = new HashSet<Favorite>();

    public User( string name, string login, string password )
    {
        Name = name;
        Login = login;
        Password = password;
    }
}
