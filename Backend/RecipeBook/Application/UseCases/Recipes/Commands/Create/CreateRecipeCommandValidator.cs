using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.UseCases.Recipes.Commands.Create;

public class CreateRecipeCommandValidator : AbstractValidator<CreateRecipeCommand>
{
    private readonly IUserRepository _repository;

    public CreateRecipeCommandValidator( IUserRepository repo )
    {
        _repository = repo;

        RuleFor( r => r.AuthorId )
            .NotEmpty().WithMessage( "Идентификатор автора обязателен." )
            .MustAsync( UserIsExists ).WithMessage( $"Пользователь не найден." )
            .GreaterThan( 0 ).WithMessage( "Идентификатор должен быть положительным числом." );

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
            .NotEmpty().WithMessage( "Название картинки обязательно." )
            .MaximumLength( 100 ).WithMessage( "Название картинки не может превышать 100 символов." );

        RuleFor( r => r.Steps )
            .NotEmpty().WithMessage( "Шаги для приготовления обязательны." );

        RuleFor( r => r.Ingredients )
            .NotEmpty().WithMessage( "Ингредиенты для приготовления обязательны." );

        RuleFor( r => r.Tags )
            .NotEmpty().WithMessage( "Теги обязательны." );
    }

    private async Task<bool> UserIsExists( int id, CancellationToken cancellationToken )
    {
        return await _repository.ContainsAsync( u => u.Id == id );
    }
}
