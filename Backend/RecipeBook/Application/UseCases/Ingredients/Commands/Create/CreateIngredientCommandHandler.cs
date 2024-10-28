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
) : ICommandHandler<CreateIngredientCommand, ResultT<Ingredient>>
{
    public async Task<ResultT<Ingredient>> Handle( CreateIngredientCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return ResultT<Ingredient>.Failure( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        Ingredient ingredient = mapper.Map<Ingredient>( command );

        await ingredientRepository.Create( ingredient );

        return ResultT<Ingredient>.Success( ingredient, $"Ингредиент {ingredient.Title} успешно добавлен." );
    }
}
