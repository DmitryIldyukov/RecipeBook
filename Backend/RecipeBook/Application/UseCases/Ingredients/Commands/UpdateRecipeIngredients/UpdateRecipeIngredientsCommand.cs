using Application.UseCases.Recipes.Dtos;
using Domain.Entities;

namespace Application.UseCases.Ingredients.Commands.UpdateRecipeIngredients;

public class UpdateRecipeIngredientsCommand
{
    public Recipe Recipe { get; init; }
    public ICollection<RecipeIngredientDto> Ingredients { get; init; }
}
