using Domain.Entities;

namespace Application.UseCases.Steps.Commands.Create;

public class CreateStepCommand
{
    public Recipe Recipe { get; init; }
    public string Description { get; init; }
}
