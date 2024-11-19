using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.UseCases.Recipes.Dtos;
using Application.UseCases.Tags.Commands.Create;
using Application.UseCases.Tags.Commands.Delete;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Tags.Commands.UpdateRecipeTags;

public class UpdateRecipeTagsCommandHandler(
    IValidator<UpdateRecipeTagsCommand> validator,
    ICommandHandler<CreateTagCommand, Result> createTagHandler,
    ICommandHandler<DeleteTagsCommand, Result> deleteTagHandler
) : ICommandHandler<UpdateRecipeTagsCommand, Result>
{
    public async Task<Result> Handle( UpdateRecipeTagsCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return Result.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        ICollection<Tag> recipeTags = command.Recipe.Tags;

        Result removeResult = await RemoveTags( recipeTags, command );
        if ( !removeResult.IsSuccess )
        {
            return Result.Fail( removeResult.ErrorMessages );
        }

        Result addResult = await AddTags( recipeTags, command );
        if ( !addResult.IsSuccess )
        {
            return Result.Fail( addResult.ErrorMessages );
        }

        return Result.Success();
    }

    private async Task<Result> RemoveTags( ICollection<Tag> recipeTags, UpdateRecipeTagsCommand command )
    {
        List<Tag> tagsToRemove = recipeTags
            .Where( tag => !command.Tags.Any( t => t.Name == tag.Name ) )
            .ToList();

        DeleteTagsCommand deleteTagsCommand = new DeleteTagsCommand()
        {
            RecipeId = command.Recipe.Id,
            Tags = tagsToRemove
        };

        return await deleteTagHandler.Handle( deleteTagsCommand );
    }

    private async Task<Result> AddTags( ICollection<Tag> recipeTags, UpdateRecipeTagsCommand command )
    {
        foreach ( RecipeTagDto tag in command.Tags )
        {
            CreateTagCommand createTagCommand = new CreateTagCommand()
            {
                Recipe = command.Recipe,
                Name = tag.Name
            };
            Result createResult = await createTagHandler.Handle( createTagCommand );

            if ( !createResult.IsSuccess )
            {
                return Result.Fail( createResult.ErrorMessages );
            }
        }

        return Result.Success();
    }
}
