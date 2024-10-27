using FluentValidation;

namespace Application.UseCases.Commands.Recipes.Create;

public class CreateRecipeCommandValidator : AbstractValidator<CreateRecipeCommand>
{
    public CreateRecipeCommandValidator()
    {
        RuleFor( r => r.AuthorId )
            .NotEmpty().WithMessage( "Id автора обязателен." );

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
    }
}
