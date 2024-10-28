using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Tags.Commands.Create;

public class CreateTagCommandHandler(
    ITagRepository tagRepository, IValidator<CreateTagCommand> validator
) : ICommandHandler<CreateTagCommand, ResultT<Tag>>
{
    public async Task<ResultT<Tag>> Handle( CreateTagCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return ResultT<Tag>.Failure( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        Tag tag = await tagRepository.GetByName( command.Name );

        if ( tag is null )
        {
            tag = new Tag( command.Name );

            await tagRepository.Create( tag );

            return ResultT<Tag>.Success( tag, $"Тэг {tag.Name} успешно добавлен." );
        }

        return ResultT<Tag>.Success( tag, $"Тэг {tag.Name} найден." );
    }
}
