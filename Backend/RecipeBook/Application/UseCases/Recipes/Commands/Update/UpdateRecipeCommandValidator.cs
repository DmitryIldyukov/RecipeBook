using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.UseCases.Recipes.Commands.Update;

public class UpdateRecipeCommandValidator : AbstractValidator<UpdateRecipeCommand>
{
    private readonly IRecipeRepository _recipeRepository;

    public UpdateRecipeCommandValidator( IRecipeRepository recipeRepository )
    {
        _recipeRepository = recipeRepository;

        RuleFor( s => s.RecipeId )
            .NotNull().WithMessage( "Идентификатор рецепта обязателен." )
            .GreaterThan( 0 ).WithMessage( "Идентификатор должен быть положительным числом." )
            .MustAsync( RecipeExists ).WithMessage( r => $"Рецепт с Id {r.RecipeId} не найден." );

        RuleFor( r => r.Name )
            .NotEmpty().WithMessage( "Название рецепта обязательно." )
            .MaximumLength( 100 ).WithMessage( "Название рецепта не может превышать 100 символов." );

        RuleFor( r => r.Description )
            .NotEmpty().WithMessage( "Описание обязательно." )
            .MaximumLength( 150 ).WithMessage( "Описание не может превышать 150 символов." );

        RuleFor( r => r.CookTime )
            .GreaterThan( 0 ).WithMessage( "Время готовки должно быть больше 0." );

        RuleFor( r => r.PortionCount )
            .GreaterThan( 0 ).WithMessage( "Количество порций должно быть больше 0." );

        RuleFor( r => r.ImageName )
            .NotEmpty().WithMessage( "Картинка обязательно." )
            .MaximumLength( 100 ).WithMessage( "Название картинки не может превышать 100 символов." );

        RuleFor( r => r.Steps )
            .NotEmpty().WithMessage( "Шаги для приготовления обязательны." );

        RuleFor( r => r.Ingredients )
            .NotEmpty().WithMessage( "Ингредиенты для приготовления обязательны." );

        RuleFor( r => r.Tags )
            .NotEmpty().WithMessage( "Теги обязательны." );
    }

    private async Task<bool> RecipeExists( int recipeId, CancellationToken cancellationToken )
    {
        return await _recipeRepository.ContainsAsync( r => r.Id == recipeId );
    }
}
