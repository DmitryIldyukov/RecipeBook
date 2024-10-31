using FluentValidation;

namespace Application.UseCases.Tags.Commands.UpdateRecipeTags;

public class UpdateRecipeTagsCommandValidator : AbstractValidator<UpdateRecipeTagsCommand>
{
    public UpdateRecipeTagsCommandValidator()
    {
        RuleFor( command => command.Recipe )
            .NotNull().WithMessage( "Рецепт обязателен." );

        RuleFor( command => command.Tags )
            .NotEmpty().WithMessage( "Тэги обязательны." );
    }
}
