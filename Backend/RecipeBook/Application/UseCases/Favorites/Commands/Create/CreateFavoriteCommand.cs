namespace Application.UseCases.Favorites.Commands.Create;

public class CreateFavoriteCommand
{
    public int UserId { get; init; }
    public int RecipeId { get; init; }
}
