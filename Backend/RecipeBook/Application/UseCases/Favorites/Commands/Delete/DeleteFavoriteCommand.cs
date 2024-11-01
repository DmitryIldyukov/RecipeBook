namespace Application.UseCases.Favorites.Commands.Delete;

public class DeleteFavoriteCommand
{
    public int UserId { get; init; }
    public int FavoriteId { get; init; }
}
