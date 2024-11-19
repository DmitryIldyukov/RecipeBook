using Domain.Entities;

namespace Application.UseCases.Tags.Commands.Create;

public record CreateTagCommand
{
    public Recipe Recipe { get; set; }
    public string Name { get; init; }
}
