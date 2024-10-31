using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Ingredients.Commands.UpdateIngredient;

public class UpdateIngredientCommandHandler(
    IIngredientRepository ingredientRepository,
    IValidator<UpdateIngredientCommand> validator
) : ICommandHandler<UpdateIngredientCommand, Result>
{
    public async Task<Result> Handle( UpdateIngredientCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return Result.Failure( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        Ingredient ingredient = await ingredientRepository.GetById( command.IngredientId );

        if ( ingredient is null )
        {
            return Result.Failure( $"Ингредиент с Id {command.IngredientId} не найден." );
        }

        ingredient.Title = command.Title;
        ingredient.Description = command.Description;

        return Result.Success();
    }
}
