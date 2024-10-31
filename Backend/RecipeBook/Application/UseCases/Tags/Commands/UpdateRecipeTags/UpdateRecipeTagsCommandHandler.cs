using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using Application.UseCases.Recipes.Dtos;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Tags.Commands.UpdateRecipeTags;

public class UpdateRecipeTagsCommandHandler(
    ITagRepository tagRepository,
    IValidator<UpdateRecipeTagsCommand> validator
) : ICommandHandler<UpdateRecipeTagsCommand, Result>
{
    public async Task<Result> Handle( UpdateRecipeTagsCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return Result.Failure( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        ICollection<Tag> recipeTags = command.Recipe.Tags;

        await RemoveTags( recipeTags, command );

        await AddTags( recipeTags, command );

        return Result.Success();
    }

    private async Task RemoveTags( ICollection<Tag> recipeTags, UpdateRecipeTagsCommand command )
    {
        List<Tag> tagsToRemove = new List<Tag>();

        foreach ( Tag tag in recipeTags.ToList() )
        {
            if ( !command.Tags.Any( t => t.Name == tag.Name ) )
            {
                tagsToRemove.Add( tag );
            }
        }

        foreach ( Tag tag in tagsToRemove )
        {
            recipeTags.Remove( tag );

            bool isUsedInOtherRecipes = await tagRepository.IsUsedInOtherRecipes( tag.Id, command.Recipe.Id );
            if ( !isUsedInOtherRecipes )
            {
                tagRepository.Delete( tag );
            }
        }
    }

    private async Task AddTags( ICollection<Tag> recipeTags, UpdateRecipeTagsCommand command )
    {
        foreach ( RecipeTagDto tag in command.Tags )
        {
            Tag tagEntity = await tagRepository.GetByName( tag.Name );
            if ( tagEntity is null )
            {
                tagEntity = new Tag( tag.Name );
                await tagRepository.Create( tagEntity );
            }

            if ( !recipeTags.Any( t => t.Id == tagEntity.Id ) )
            {
                recipeTags.Add( tagEntity );
            }
        }
    }
}
