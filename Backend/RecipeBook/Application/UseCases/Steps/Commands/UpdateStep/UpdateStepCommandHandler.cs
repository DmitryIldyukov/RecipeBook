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
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return Result.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        Step step = await stepRepository.GetById( command.StepId );

        step.Description = command.Description;

        return Result.Success();
    }
}
