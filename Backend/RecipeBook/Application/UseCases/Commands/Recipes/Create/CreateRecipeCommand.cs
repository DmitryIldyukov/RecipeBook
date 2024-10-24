using Application.UseCases.Commands.Dtos.Ingredients;
using Application.UseCases.Commands.Dtos.Steps;
using Application.UseCases.Commands.Dtos.Tags;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Commands.Recipes.Create;

public class CreateRecipeCommand
{
    public int AuthorId { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public int CookTime { get; init; }
    public int PortionCount { get; init; }
    public string ImageName { get; init; }
    public IFormFile ImageFile { get; init; }
    public ICollection<TagDto> Tags { get; init; }
    public ICollection<StepDto> Steps { get; init; }
    public ICollection<IngredientDto> Ingredients { get; init; }
}
