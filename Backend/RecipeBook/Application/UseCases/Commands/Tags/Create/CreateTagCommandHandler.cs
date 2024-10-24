using Application.Common.CQRS.Command;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Commands.Tags.Create;

public class CreateTagCommandHandler(
    ITagRepository tagRepository, IValidator<CreateTagCommand> validator
) : ICommandHandler<CreateTagCommand, Tag>
{
    public async Task<Tag> Handle( CreateTagCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            throw new ValidationException( validationResult.Errors );
        }

        Tag tag = await tagRepository.GetByName( command.Name );

        if ( tag is null )
        {
            tag = new Tag( command.Name );

            await tagRepository.Create( tag );

            return tag;
        }

        return tag;
    }
}
