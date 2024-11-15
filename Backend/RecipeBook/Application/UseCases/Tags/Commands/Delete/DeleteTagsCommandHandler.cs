using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using Application.UseCases.Steps.Commands.UpdateStep;
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
        Result validationResult = await ValidateCommandAsync( command );
        if ( !validationResult.IsSuccess )
        {
            return validationResult;
        }

        Recipe recipe = await recipeRepository.GetById( command.RecipeId );
        if ( recipe is null )
        {
            return Result.Fail( $"Рецет с Id {command.RecipeId} не найден." );
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

    private async Task<Result> ValidateCommandAsync( DeleteTagsCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return Result.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        return Result.Success();
    }
}
