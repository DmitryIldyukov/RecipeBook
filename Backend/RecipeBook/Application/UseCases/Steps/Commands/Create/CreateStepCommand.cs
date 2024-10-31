namespace Application.UseCases.Steps.Commands.Create;

public record CreateStepCommand
{
    public int RecipeId { get; init; }
    public string Description { get; init; }
}
