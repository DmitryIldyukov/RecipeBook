using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Steps.Commands.Create;

public class CreateStepCommandHandler(
    IStepRepository stepRepository,
    IValidator<CreateStepCommand> validator
) : ICommandHandler<CreateStepCommand, Result>
{
    public async Task<Result> Handle( CreateStepCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return Result.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        Step step = new( command.Recipe.Id, command.Description );

        await stepRepository.Create( step );

        command.Recipe.Steps.Add( step );

        return Result.Success( "Шаг успешно добавлен." );
    }
}
