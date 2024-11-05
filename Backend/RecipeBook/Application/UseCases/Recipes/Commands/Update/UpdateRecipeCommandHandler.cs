using Application.Common.CQRS.Command;
using Application.Common.FileHelper;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.UseCases.Ingredients.Commands.UpdateRecipeIngredients;
using Application.UseCases.Recipes.Dtos;
using Application.UseCases.Steps.Commands.UpdateRecipeSteps;
using Application.UseCases.Tags.Commands.UpdateRecipeTags;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Application.UseCases.Recipes.Commands.Update;

public class UpdateRecipeCommandHandler(
    IRecipeRepository recipeRepository,
    IUnitOfWork unitOfWork,
    IValidator<UpdateRecipeCommand> validator,
    IFileHelper fileHelper,
    IConfiguration configuration,
    ICommandHandler<UpdateRecipeTagsCommand, Result> updateRecipeTagsHandler,
    ICommandHandler<UpdateRecipeIngredientsCommand, Result> updateRecipeIngredientsHandler,
    ICommandHandler<UpdateRecipeStepsCommand, Result> updateRecipeStepsHandler
) : ICommandHandler<UpdateRecipeCommand, Result>
{
    public async Task<Result> Handle( UpdateRecipeCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return Result.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        Recipe recipe = await recipeRepository.GetById( command.RecipeId );

        if ( recipe is null )
        {
            return Result.Fail( $"Рецепт с Id {command.RecipeId} не найден." );
        }

        recipe.Name = command.Name;
        recipe.Description = command.Description;
        recipe.CookTime = command.CookTime;
        recipe.PortionCount = command.PortionCount;

        Result updateTagsResult = await UpdateTags( recipe, command.Tags );
        if ( !updateTagsResult.IsSuccess )
        {
            return Result.Fail( updateTagsResult.ErrorMessages );
        }

        await UpdateTags( recipe, command.Tags );

        await UpdateSteps( recipe, command.Steps );

        await UpdateIngredients( recipe, command.Ingredients );

        if ( command.ImageFile is not null )
        {
            string oldImageName = recipe.ImageName;
            DeleteOldImage( recipe, oldImageName );

            recipe.ImageName = command.ImageName;
            SaveImage( recipe, command.ImageFile );
        }

        await unitOfWork.Commit();

        return Result.Success();
    }

    private async Task<Result> UpdateTags( Recipe recipe, ICollection<RecipeTagDto> tags )
    {
        UpdateRecipeTagsCommand updateRecipeTagsCommand = new UpdateRecipeTagsCommand()
        {
            Recipe = recipe,
            Tags = tags
        };

        Result result = await updateRecipeTagsHandler.Handle( updateRecipeTagsCommand );

        return result.IsSuccess ? Result.Success() : Result.Fail( result.ErrorMessages );
    }

    private async Task<Result> UpdateSteps( Recipe recipe, ICollection<RecipeStepDto> steps )
    {
        UpdateRecipeStepsCommand updateRecipeStepsCommand = new UpdateRecipeStepsCommand()
        {
            Recipe = recipe,
            Steps = steps
        };

        Result result = await updateRecipeStepsHandler.Handle( updateRecipeStepsCommand );

        return result.IsSuccess ? Result.Success() : Result.Fail( result.ErrorMessages );
    }

    private async Task<Result> UpdateIngredients( Recipe recipe, ICollection<RecipeIngredientDto> ingredients )
    {
        UpdateRecipeIngredientsCommand updateRecipeIngredientsCommand = new UpdateRecipeIngredientsCommand()
        {
            Recipe = recipe,
            Ingredients = ingredients
        };

        Result result = await updateRecipeIngredientsHandler.Handle( updateRecipeIngredientsCommand );

        return result.IsSuccess ? Result.Success() : Result.Fail( result.ErrorMessages );
    }

    private void DeleteOldImage( Recipe recipe, string oldImageName )
    {
        string oldFileExtension = Path.GetExtension( oldImageName );
        string oldFileNameOnDisk = recipe.Id + oldFileExtension;
        fileHelper.Delete( oldFileNameOnDisk );
    }

    private void SaveImage( Recipe recipe, IFormFile image )
    {
        string fileExtension = Path.GetExtension( recipe.ImageName );
        string fileNameOnDisk = recipe.Id + fileExtension;
        fileHelper.Save( configuration.GetSection( "RecipeImages" ).Value, fileNameOnDisk, image.OpenReadStream() );
    }
}
