using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.Interfaces.Repositories;
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
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return Result.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        Tag tag = await tagRepository.GetByName( command.Name );

        if ( tag is null )
        {
            tag = new Tag( command.Name );

            await tagRepository.Create( tag );

            command.Recipe.Tags.Add( tag );

            return Result.Success( $"Тэг {tag.Name} успешно добавлен." );
        }

        return Result.Success( $"Тэг {tag.Name} найден." );
    }
}
