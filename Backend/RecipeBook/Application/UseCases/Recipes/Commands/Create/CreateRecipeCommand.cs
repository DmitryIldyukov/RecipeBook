using Application.UseCases.Recipes.Dtos;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Recipes.Commands.Create;

public record CreateRecipeCommand
{
    public int AuthorId { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public int CookTime { get; init; }
    public int PortionCount { get; init; }
    public string ImageName { get; init; }
    public IFormFile ImageFile { get; init; }
    public ICollection<RecipeTagDto> Tags { get; init; }
    public ICollection<RecipeStepDto> Steps { get; init; }
    public ICollection<RecipeIngredientDto> Ingredients { get; init; }
}
