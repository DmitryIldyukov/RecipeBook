namespace Application.UseCases.Likes.Commands.Delete;

public record DeleteLikeCommand
{
    public int RecipeId { get; init; }
    public int UserId { get; init; }
}
