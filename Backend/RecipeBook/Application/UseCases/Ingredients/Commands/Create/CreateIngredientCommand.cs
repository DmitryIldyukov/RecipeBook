using Domain.Entities;

namespace Application.UseCases.Ingredients.Commands.Create;

public class CreateIngredientCommand
{
    public Recipe Recipe { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
}
