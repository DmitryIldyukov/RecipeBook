using Application.UseCases.Recipes.Dtos;
using Domain.Entities;

namespace Application.UseCases.Steps.Commands.UpdateRecipeSteps;

public class UpdateRecipeStepsCommand
{
    public Recipe Recipe { get; init; }
    public ICollection<RecipeStepDto> Steps { get; init; }
}
