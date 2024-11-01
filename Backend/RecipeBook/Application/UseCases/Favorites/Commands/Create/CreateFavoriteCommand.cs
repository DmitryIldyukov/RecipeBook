namespace Application.UseCases.Favorites.Commands.Create;

public record CreateFavoriteCommand
{
    public int UserId { get; init; }
    public int RecipeId { get; init; }
}
