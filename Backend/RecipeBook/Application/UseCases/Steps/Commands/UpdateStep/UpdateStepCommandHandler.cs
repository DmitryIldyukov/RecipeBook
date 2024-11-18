using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Steps.Commands.UpdateStep;

public class UpdateStepCommandHandler(
    IStepRepository stepRepository,
    IValidator<UpdateStepCommand> validator
) : ICommandHandler<UpdateStepCommand, Result>
{
    public async Task<Result> Handle( UpdateStepCommand command )
    {
        Step step = await stepRepository.GetById( command.StepId );

        Result validationResult = await ValidateAsync( command, step );
        if ( !validationResult.IsSuccess )
        {
            return validationResult;
        }

        step.Description = command.Description;

        return Result.Success();
    }

    private async Task<Result> ValidateAsync( UpdateStepCommand command, Step step )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return Result.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        if ( step is null )
        {
            return Result.Fail( $"Шаг с Id {command.StepId} не найден." );
        }

        return Result.Success();
    }
}
