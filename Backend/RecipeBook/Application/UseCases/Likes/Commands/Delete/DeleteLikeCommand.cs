namespace Application.UseCases.Likes.Commands.Delete;

public class DeleteLikeCommand
{
    public int LikeId { get; init; }
    public int UserId { get; init; }
}
