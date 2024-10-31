using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Tags.Commands.Delete;

public class DeleteTagsCommandHandler(
    IValidator<DeleteTagsCommand> validator,
    ITagRepository tagRepository
) : ICommandHandler<DeleteTagsCommand, Result>
{
    public async Task<Result> Handle( DeleteTagsCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return Result.Failure( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        foreach ( Tag tag in command.Tags )
        {
            if ( !await tagRepository.IsUsedInOtherRecipes( tag.Id, command.RecipeId ) )
            {
                tagRepository.Delete( tag );
            }
        }

        return Result.Success();
    }
}
