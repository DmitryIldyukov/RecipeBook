using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using Application.UseCases.Steps.Commands.UpdateStep;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Tags.Commands.Create;

public class CreateTagCommandHandler(
    ITagRepository tagRepository,
    IValidator<CreateTagCommand> validator
    ) : ICommandHandler<CreateTagCommand, Result>
{
    public async Task<Result> Handle( CreateTagCommand command )
    {
        Result validationResult = await ValidateCommandAsync( command );
        if ( !validationResult.IsSuccess )
        {
            return validationResult;
        }

        string tagName = command.Name.ToLower().Trim();

        Tag tag = await tagRepository.GetByName( tagName );

        if ( tag is null )
        {
            tag = new Tag( tagName );

            await tagRepository.Create( tag );
        }

        command.Recipe.Tags.Add( tag );

        return Result.Success();
    }

    private async Task<Result> ValidateCommandAsync( CreateTagCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return Result.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        return Result.Success();
    }
}
