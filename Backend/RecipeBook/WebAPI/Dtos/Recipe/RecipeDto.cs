using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos.Igredient;
using WebAPI.Dtos.Step;
using WebAPI.Dtos.Tag;

namespace WebAPI.Dtos.Recipe;

public class RecipeDto
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
