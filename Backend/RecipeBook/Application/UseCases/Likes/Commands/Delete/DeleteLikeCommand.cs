namespace Application.UseCases.Likes.Commands.Delete;

public record DeleteLikeCommand
{
    public int LikeId { get; init; }
    public int UserId { get; init; }
}
