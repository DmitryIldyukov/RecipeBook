using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Tags.Commands.Delete;

public class DeleteTagsCommandHandler(
    IValidator<DeleteTagsCommand> validator,
    ITagRepository tagRepository,
    IRecipeRepository recipeRepository
) : ICommandHandler<DeleteTagsCommand, Result>
{
    public async Task<Result> Handle( DeleteTagsCommand command )
    {
        Recipe recipe = await recipeRepository.GetById( command.RecipeId );

        Result validationResult = await ValidateAsync( command, recipe );
        if ( !validationResult.IsSuccess )
        {
            return validationResult;
        }

        foreach ( Tag tag in command.Tags )
        {
            recipe.Tags.Remove( tag );
            if ( !await tagRepository.IsUsedInMultipleRecipes( tag.Id ) )
            {
                tagRepository.Delete( tag );
            }
        }

        return Result.Success();
    }

    private async Task<Result> ValidateAsync( DeleteTagsCommand command, Recipe recipe )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return Result.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        if ( recipe is null )
        {
            return Result.Fail( $"Рецет с Id {command.RecipeId} не найден." );
        }

        return Result.Success();
    }
}
