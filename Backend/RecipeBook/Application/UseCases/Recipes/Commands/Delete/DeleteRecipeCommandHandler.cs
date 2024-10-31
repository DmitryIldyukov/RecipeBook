using Application.Common.CQRS.Command;
using Application.Common.FileHelper;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.UseCases.Tags.Commands.Delete;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Recipes.Commands.Delete;

public class DeleteRecipeCommandHandler(
    IRecipeRepository recipeRepository,
    IValidator<DeleteRecipeCommand> validator,
    ICommandHandler<DeleteTagsCommand, Result> deleteTagsHandler,
    IUnitOfWork unitOfWork,
    IFileHelper fileHelper
) : ICommandHandler<DeleteRecipeCommand, Result>
{
    public async Task<Result> Handle( DeleteRecipeCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return Result.Failure( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        Recipe recipe = await recipeRepository.GetById( command.RecipeId );
        if ( recipe is null )
        {
            return Result.Failure( $"Рецепт с Id {command.RecipeId} не найден." );
        }

        DeleteTagsCommand deleteTagsCommand = new DeleteTagsCommand()
        {
            Tags = recipe.Tags
        };
        await deleteTagsHandler.Handle( deleteTagsCommand );

        recipeRepository.Delete( recipe );
        await unitOfWork.Commit();

        fileHelper.Delete( recipe.ImageName );

        return Result.Success();
    }
}
