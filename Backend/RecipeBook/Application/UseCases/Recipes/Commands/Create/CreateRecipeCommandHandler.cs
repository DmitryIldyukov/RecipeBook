using Application.Common.CQRS.Command;
using Application.Common.FileHelper;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.UseCases.Ingredients.Commands.Create;
using Application.UseCases.Recipes.Dtos;
using Application.UseCases.Steps.Commands.Create;
using Application.UseCases.Tags.Commands.Create;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Application.UseCases.Recipes.Commands.Create;

public class CreateRecipeCommandHandler(
    IRecipeRepository recipeRepository,
    IUnitOfWork unitOfWork,
    ICommandHandler<CreateTagCommand, Result> createTagHandler,
    ICommandHandler<CreateStepCommand, Result> createStepHandler,
    ICommandHandler<CreateIngredientCommand, Result> createIngredientHandler,
    IValidator<CreateRecipeCommand> validator,
    IFileHelper fileHelper,
    IConfiguration configuration
) : ICommandHandler<CreateRecipeCommand, Result>
{
    public async Task<Result> Handle( CreateRecipeCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return Result.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        Recipe recipe = new Recipe(
            command.AuthorId,
            command.Name, command.Description,
            command.CookTime,
            command.PortionCount,
            command.ImageName
        );

        Result addTagsResult = await AddTags( recipe, command.Tags );
        if ( !addTagsResult.IsSuccess )
        {
            return Result.Fail( addTagsResult.ErrorMessages );
        }

        Result addStepsResult = await AddSteps( recipe, command.Steps );
        if ( !addStepsResult.IsSuccess )
        {
            return Result.Fail( addStepsResult.ErrorMessages );
        }

        Result addIngredientsResult = await AddIngredients( recipe, command.Ingredients );
        if ( !addIngredientsResult.IsSuccess )
        {
            return Result.Fail( addIngredientsResult.ErrorMessages );
        }

        await recipeRepository.Create( recipe );

        await unitOfWork.Commit();

        SaveImage( recipe, command.ImageFile );

        return Result.Success( "Рецепт успешно добавлен." );
    }

    private async Task<Result> AddTags( Recipe recipe, ICollection<RecipeTagDto> tags )
    {
        foreach ( RecipeTagDto tag in tags )
        {
            CreateTagCommand createTagCommand = new CreateTagCommand()
            {
                Recipe = recipe,
                Name = tag.Name,
            };
            Result tagResult = await createTagHandler.Handle( createTagCommand );

            if ( !tagResult.IsSuccess )
            {
                return Result.Fail( tagResult.ErrorMessages );
            }
        }

        return Result.Success( "Тэги успешно добавлены." );
    }

    private async Task<Result> AddSteps( Recipe recipe, ICollection<RecipeStepDto> steps )
    {
        foreach ( RecipeStepDto step in steps )
        {
            CreateStepCommand createStepCommand = new CreateStepCommand()
            {
                Recipe = recipe,
                Description = step.Description,
            };

            Result stepResult = await createStepHandler.Handle( createStepCommand );

            if ( !stepResult.IsSuccess )
            {
                return Result.Fail( stepResult.ErrorMessages );
            }
        }

        return Result.Success( "Шаги успешно добавлены." );
    }

    private async Task<Result> AddIngredients( Recipe recipe, ICollection<RecipeIngredientDto> ingredients )
    {
        foreach ( RecipeIngredientDto ingredient in ingredients )
        {
            CreateIngredientCommand createIngredientCommand = new CreateIngredientCommand()
            {
                Recipe = recipe,
                Title = ingredient.Title,
                Description = ingredient.Description,
            };

            Result ingredientResult = await createIngredientHandler.Handle( createIngredientCommand );

            if ( !ingredientResult.IsSuccess )
            {
                return Result.Fail( ingredientResult.ErrorMessages );
            }
        }

        return Result.Success( "Ингредиенты успешно добавлены." );
    }

    private void SaveImage( Recipe recipe, IFormFile image )
    {
        string fileExtension = Path.GetExtension( recipe.ImageName );
        string fileNameOnDisk = recipe.Id + fileExtension;
        fileHelper.Save( configuration.GetSection( "RecipeImages" ).Value, fileNameOnDisk, image.OpenReadStream() );
    }
}
