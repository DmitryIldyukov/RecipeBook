using Application.Common.CQRS.Command;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Commands.Ingredients.Create;

public class CreateIngredientCommandHandler(
    IIngredientRepository ingredientRepository,
    IValidator<CreateIngredientCommand> validator,
    IMapper mapper
) : ICommandHandler<CreateIngredientCommand, Ingredient>
{
    public async Task<Ingredient> Handle( CreateIngredientCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            throw new ValidationException( validationResult.Errors );
        }

        Ingredient ingredient = mapper.Map<Ingredient>( command );

        await ingredientRepository.Create( ingredient );

        return ingredient;
    }
}
