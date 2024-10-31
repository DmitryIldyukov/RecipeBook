using FluentValidation;

namespace Application.UseCases.Tags.Commands.Delete;

public class DeleteTagsCommandValidator : AbstractValidator<DeleteTagsCommand>
{
    public DeleteTagsCommandValidator()
    {
        RuleFor( t => t.RecipeId )
            .NotEmpty().WithMessage( "Id рецепта обязателен." );
    }
}
