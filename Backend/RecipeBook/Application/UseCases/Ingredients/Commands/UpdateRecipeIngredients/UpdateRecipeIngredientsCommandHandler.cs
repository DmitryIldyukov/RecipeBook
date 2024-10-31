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
    ICommandHandler<CreateIngredientCommand, ResultT<Ingredient>> createIngredientHandler,
    ICommandHandler<UpdateIngredientCommand, Result> updateIngredientHandler,
    IValidator<UpdateRecipeIngredientsCommand> validator
) : ICommandHandler<UpdateRecipeIngredientsCommand, Result>
{
    public async Task<Result> Handle( UpdateRecipeIngredientsCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return Result.Failure( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        ICollection<Ingredient> recipeIngredients = command.Recipe.Ingredients;
        List<Ingredient> ingredientsToRemove = new List<Ingredient>();

        foreach ( Ingredient ingredient in recipeIngredients )
        {
            bool existsInCommand = command.Ingredients.Any( i => i.IngredientId is not null && i.IngredientId == ingredient.Id );
            if ( !existsInCommand )
            {
                ingredientsToRemove.Add( ingredient );
            }
        }


        foreach ( Ingredient ingredient in ingredientsToRemove )
        {
            recipeIngredients.Remove( ingredient );
            ingredientRepository.Delete( ingredient );
        }

        foreach ( RecipeIngredientDto ingredientDto in command.Ingredients )
        {
            if ( ingredientDto.IngredientId is not null )
            {
                Ingredient ingredientEntity = recipeIngredients.FirstOrDefault( i => i.Id == ingredientDto.IngredientId );

                if ( ingredientEntity is null )
                {
                    return Result.Failure( $"Ингредиент с Id {ingredientDto.IngredientId} не найден." );
                }

                UpdateIngredientCommand updateIngredientCommand = new UpdateIngredientCommand()
                {
                    IngredientId = ingredientEntity.Id,
                    Title = ingredientDto.Title,
                    Description = ingredientDto.Description
                };

                Result updateIngredientResult = await updateIngredientHandler.Handle( updateIngredientCommand );
                if ( !updateIngredientResult.IsSuccess )
                {
                    return Result.Failure( updateIngredientResult.ErrorMessages );
                }
            }
            else
            {
                CreateIngredientCommand createIngredientCommand = new CreateIngredientCommand()
                {
                    RecipeId = command.Recipe.Id,
                    Title = ingredientDto.Title,
                    Description = ingredientDto.Description
                };

                ResultT<Ingredient> createIngredientResult = await createIngredientHandler.Handle( createIngredientCommand );
                if ( !createIngredientResult.IsSuccess )
                {
                    return Result.Failure( createIngredientResult.ErrorMessages );
                }

                recipeIngredients.Add( createIngredientResult.Value );
            }
        }

        return Result.Success();
    }
}
