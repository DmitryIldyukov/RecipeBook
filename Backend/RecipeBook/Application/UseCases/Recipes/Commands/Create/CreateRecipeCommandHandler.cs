using Application.Common.CQRS.Command;
using Application.Common.FileHelper;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.UseCases.Ingredients.Commands.Create;
using Application.UseCases.Recipes.Dtos;
using Application.UseCases.Steps.Commands.Create;
using Application.UseCases.Tags.Commands.Create;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Application.UseCases.Recipes.Commands.Create;

public class CreateRecipeCommandHandler(
    IRecipeRepository recipeRepository,
    IUnitOfWork unitOfWork,
    ICommandHandler<CreateTagCommand, ResultT<Tag>> createTagHandler,
    ICommandHandler<CreateStepCommand, ResultT<Step>> createStepHandler,
    ICommandHandler<CreateIngredientCommand, ResultT<Ingredient>> createIngredientHandler,
    IValidator<CreateRecipeCommand> validator,
    IFileHelper fileHelper,
    IConfiguration configuration,
    IMapper mapper
) : ICommandHandler<CreateRecipeCommand, Result>
{
    public async Task<Result> Handle( CreateRecipeCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return Result.Failure( validationResult.Errors.Select( e => e.ErrorMessage ) );
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
            return Result.Failure( addTagsResult.ErrorMessages );
        }

        Result addStepsResult = await AddSteps( recipe, command.Steps );
        if ( !addStepsResult.IsSuccess )
        {
            return Result.Failure( addStepsResult.ErrorMessages );
        }

        Result addIngredientsResult = await AddIngredients( recipe, command.Ingredients );
        if ( !addIngredientsResult.IsSuccess )
        {
            return Result.Failure( addIngredientsResult.ErrorMessages );
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
            CreateTagCommand createTagCommand = mapper.Map<CreateTagCommand>( tag );
            ResultT<Tag> tagResult = await createTagHandler.Handle( createTagCommand );

            if ( !tagResult.IsSuccess )
            {
                return Result.Failure( tagResult.ErrorMessages );
            }

            recipe.Tags.Add( tagResult.Value );
        }

        return Result.Success( "Тэги успешно добавлены." );
    }

    private async Task<Result> AddSteps( Recipe recipe, ICollection<RecipeStepDto> steps )
    {
        foreach ( RecipeStepDto step in steps )
        {
            CreateStepCommand createStepCommand = new CreateStepCommand()
            {
                RecipeId = recipe.Id,
                Description = step.Description,
            };

            ResultT<Step> stepResult = await createStepHandler.Handle( createStepCommand );

            if ( !stepResult.IsSuccess )
            {
                return Result.Failure( stepResult.ErrorMessages );
            }

            recipe.Steps.Add( stepResult.Value );
        }

        return Result.Success( "Шаги успешно добавлены." );
    }

    private async Task<Result> AddIngredients( Recipe recipe, ICollection<RecipeIngredientDto> ingredients )
    {
        foreach ( RecipeIngredientDto ingredient in ingredients )
        {
            CreateIngredientCommand createIngredientCommand = new CreateIngredientCommand()
            {
                RecipeId = recipe.Id,
                Title = ingredient.Title,
                Description = ingredient.Description,
            };

            ResultT<Ingredient> ingredientResult = await createIngredientHandler.Handle( createIngredientCommand );

            if ( !ingredientResult.IsSuccess )
            {
                return Result.Failure( ingredientResult.ErrorMessages );
            }

            recipe.Ingredients.Add( ingredientResult.Value );
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
