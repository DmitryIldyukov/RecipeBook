namespace Application.UseCases.Tags.Commands.Create;

public record CreateTagCommand
{
    public string Name { get; init; }
}
