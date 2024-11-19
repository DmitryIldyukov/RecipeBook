namespace Application.UseCases.Steps.Commands.UpdateStep;

public record UpdateStepCommand
{
    public int StepId { get; init; }
    public string Description { get; init; }
}
