using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Ingredients.Commands.Create;

public class CreateIngredientCommandHandler(
    IIngredientRepository ingredientRepository,
    IValidator<CreateIngredientCommand> validator,
    IMapper mapper
) : ICommandHandler<CreateIngredientCommand, Result>
{
    public async Task<Result> Handle( CreateIngredientCommand command )
    {
        Result validationResult = await ValidateCommandAsync( command );
        if ( !validationResult.IsSuccess )
        {
            return validationResult;
        }

        Ingredient ingredient = mapper.Map<Ingredient>( command );

        await ingredientRepository.Create( ingredient );

        command.Recipe.Ingredients.Add( ingredient );

        return Result.Success( $"Ингредиент {ingredient.Title} успешно добавлен." );
    }

    private async Task<Result> ValidateCommandAsync( CreateIngredientCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return Result.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        return Result.Success();
    }
}
