using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.UseCases.Steps.Commands.UpdateStep;

public class UpdateStepCommandValidator : AbstractValidator<UpdateStepCommand>
{
    private readonly IStepRepository _stepRepository;

    public UpdateStepCommandValidator( IStepRepository stepRepository )
    {
        _stepRepository = stepRepository;

        RuleFor( s => s.StepId )
            .NotNull().WithMessage( "Идентификатор шага обязателен." )
            .GreaterThan( 0 ).WithMessage( "Идентификатор должен быть положительным числом." )
            .MustAsync( StepExists ).WithMessage( s => $"Шаг с Id {s.StepId} не найден." );

        RuleFor( s => s.Description )
            .NotEmpty().WithMessage( "Описание шага обязательно." );
    }

    private async Task<bool> StepExists( int stepId, CancellationToken cancellationToken )
    {
        return await _stepRepository.ContainsAsync( s => s.Id == stepId );
    }
}
