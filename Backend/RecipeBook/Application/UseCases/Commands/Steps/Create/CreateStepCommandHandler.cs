using Application.Common.CQRS.Command;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Commands.Steps.Create;

public class CreateStepCommandHandler(
    IStepRepository stepRepository,
    IValidator<CreateStepCommand> validator
) : ICommandHandler<CreateStepCommand, Step>
{
    public async Task<Step> Handle( CreateStepCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            throw new ValidationException( validationResult.Errors );
        }

        Step step = new( command.RecipeId, command.Description );

        await stepRepository.Create( step );

        return step;
    }
}
