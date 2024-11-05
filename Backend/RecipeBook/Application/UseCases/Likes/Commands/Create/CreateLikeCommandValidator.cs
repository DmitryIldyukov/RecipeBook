using FluentValidation;

namespace Application.UseCases.Likes.Commands.Create;

public class CreateLikeCommandValidator : AbstractValidator<CreateLikeCommand>
{
    public CreateLikeCommandValidator()
    {
        RuleFor( f => f.RecipeId )
            .NotEmpty().WithMessage( "Идентификатор рецепта обязателен." );

        RuleFor( f => f.UserId )
            .NotEmpty().WithMessage( "Идентификатор пользователя обязателен." );
    }
}
