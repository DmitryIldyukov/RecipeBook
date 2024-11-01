namespace Application.UseCases.Likes.Commands.Create;

public class CreateLikeCommand
{
    public int UserId { get; init; }
    public int RecipeId { get; init; }
}
