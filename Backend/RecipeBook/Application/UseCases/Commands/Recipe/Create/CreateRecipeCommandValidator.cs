using FluentValidation;

namespace Application.UseCases.Commands.Recipe.Create;

public class CreateRecipeCommandValidator : AbstractValidator<CreateRecipeCommand>
{
    public CreateRecipeCommandValidator()
    {

    }
}
