using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using Application.UseCases.Ingredients.Commands.Create;
using Application.UseCases.Ingredients.Commands.UpdateIngredient;
using Application.UseCases.Recipes.Dtos;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Ingredients.Commands.UpdateRecipeIngredients;

public class UpdateRecipeIngredientsCommandHandler(
    IIngredientRepository ingredientRepository,
    ICommandHandler<CreateIngredientCommand, Result> createIngredientHandler,
    ICommandHandler<UpdateIngredientCommand, Result> updateIngredientHandler,
    IValidator<UpdateRecipeIngredientsCommand> validator
) : ICommandHandler<UpdateRecipeIngredientsCommand, Result>
{
    public async Task<Result> Handle( UpdateRecipeIngredientsCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return Result.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        ICollection<Ingredient> recipeIngredients = command.Recipe.Ingredients;
        List<Ingredient> ingredientsToRemove = IdentifyIngredientsToRemove( recipeIngredients, command.Ingredients );
        RemoveIngredientsAsync( ingredientsToRemove, recipeIngredients );

        Result updateResult = await UpdateAndCreateIngredientsAsync( command, recipeIngredients );
        if ( !updateResult.IsSuccess )
        {
            return updateResult;
        }

        return Result.Success();
    }

    private List<Ingredient> IdentifyIngredientsToRemove( ICollection<Ingredient> recipeIngredients, IEnumerable<RecipeIngredientDto> commandIngredients )
    {
        return recipeIngredients
            .Where( ingredient => !commandIngredients.Any( i => i.IngredientId == ingredient.Id ) )
            .ToList();
    }

    private void RemoveIngredientsAsync( IEnumerable<Ingredient> ingredientsToRemove, ICollection<Ingredient> recipeIngredients )
    {
        foreach ( Ingredient ingredient in ingredientsToRemove )
        {
            recipeIngredients.Remove( ingredient );
            ingredientRepository.Delete( ingredient );
        }
    }

    private async Task<Result> UpdateAndCreateIngredientsAsync( UpdateRecipeIngredientsCommand command, ICollection<Ingredient> recipeIngredients )
    {
        foreach ( RecipeIngredientDto ingredientDto in command.Ingredients )
        {
            if ( ingredientDto.IngredientId is not null )
            {
                Result updateResult = await UpdateExistingIngredientAsync( ingredientDto, recipeIngredients );
                if ( !updateResult.IsSuccess )
                {
                    return updateResult;
                }
            }
            else
            {
                Result createResult = await CreateNewIngredientAsync( ingredientDto, command.Recipe );
                if ( !createResult.IsSuccess )
                {
                    return createResult;
                }
            }
        }

        return Result.Success();
    }

    private async Task<Result> UpdateExistingIngredientAsync( RecipeIngredientDto ingredientDto, ICollection<Ingredient> recipeIngredients )
    {
        Ingredient ingredientEntity = recipeIngredients.FirstOrDefault( i => i.Id == ingredientDto.IngredientId );
        if ( ingredientEntity == null )
        {
            return Result.Fail( $"Ингредиент с Id {ingredientDto.IngredientId} не найден." );
        }

        UpdateIngredientCommand updateIngredientCommand = new UpdateIngredientCommand
        {
            IngredientId = ingredientEntity.Id,
            Title = ingredientDto.Title,
            Description = ingredientDto.Description
        };
        Result updateResult = await updateIngredientHandler.Handle( updateIngredientCommand );

        return updateResult.IsSuccess ? Result.Success() : Result.Fail( updateResult.ErrorMessages );
    }

    private async Task<Result> CreateNewIngredientAsync( RecipeIngredientDto ingredientDto, Recipe recipe )
    {
        CreateIngredientCommand createIngredientCommand = new CreateIngredientCommand
        {
            Recipe = recipe,
            Title = ingredientDto.Title,
            Description = ingredientDto.Description
        };
        Result createResult = await createIngredientHandler.Handle( createIngredientCommand );

        return createResult.IsSuccess ? Result.Success() : Result.Fail( createResult.ErrorMessages );
    }
}