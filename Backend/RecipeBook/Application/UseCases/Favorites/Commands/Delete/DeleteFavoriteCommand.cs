namespace Application.UseCases.Favorites.Commands.Delete;

public record DeleteFavoriteCommand
{
    public int UserId { get; init; }
    public int FavoriteId { get; init; }
}
