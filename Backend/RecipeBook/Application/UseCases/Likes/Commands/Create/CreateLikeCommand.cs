namespace Application.UseCases.Likes.Commands.Create;

public record CreateLikeCommand
{
    public int UserId { get; init; }
    public int RecipeId { get; init; }
}
