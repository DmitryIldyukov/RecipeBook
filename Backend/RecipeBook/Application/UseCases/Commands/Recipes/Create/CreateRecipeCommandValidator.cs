using FluentValidation;

namespace Application.UseCases.Commands.Recipes.Create;

public class CreateRecipeCommandValidator : AbstractValidator<CreateRecipeCommand>
{
    public CreateRecipeCommandValidator()
    {

    }
}
