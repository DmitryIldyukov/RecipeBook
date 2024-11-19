using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.UseCases.Ingredients.Commands.UpdateIngredient;

public class UpdateIngredientCommandValidator : AbstractValidator<UpdateIngredientCommand>
{
    private readonly IIngredientRepository _ingredientRepository;

    public UpdateIngredientCommandValidator( IIngredientRepository ingredientRepository )
    {
        _ingredientRepository = ingredientRepository;

        RuleFor( i => i.IngredientId )
            .NotNull().WithMessage( "Идентификатор ингредиента обязателен." )
            .GreaterThan( 0 ).WithMessage( "Идентификатор должен быть положительным числом." )
            .MustAsync( IngredientExists ).WithMessage( i => $"Ингредиент с Id {i.IngredientId} не найден." );

        RuleFor( i => i.Title )
            .NotEmpty().WithMessage( "Заголовок ингредиента обязателен." )
            .MaximumLength( 40 ).WithMessage( "Заголовок ингредиента не может превышать 40 символов." );

        RuleFor( i => i.Description )
            .NotEmpty().WithMessage( "Описание ингредиента обязательно." );
    }

    private async Task<bool> IngredientExists( int ingredientId, CancellationToken cancellationToken )
    {
        return await _ingredientRepository.ContainsAsync( i => i.Id == ingredientId );
    }
}
