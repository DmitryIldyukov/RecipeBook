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
) : ICommandHandler<CreateStepCommand, ResultT<Step>>
{
    public async Task<ResultT<Step>> Handle( CreateStepCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return ResultT<Step>.Failure( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        Step step = new( command.RecipeId, command.Description );

        await stepRepository.Create( step );

        return ResultT<Step>.Success( step, "Шаг успешно добавлен." );
    }
}
