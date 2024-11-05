using WebAPI.Dtos.Igredient;
using WebAPI.Dtos.Step;
using WebAPI.Dtos.Tag;

namespace WebAPI.Dtos.Recipe;

public class UpdateRecipeDto
{
    public string Name { get; init; }
    public string Description { get; init; }
    public int CookTime { get; init; }
    public int PortionCount { get; init; }
    public string ImageName { get; init; }
    public IFormFile ImageFile { get; init; }

    public ICollection<UpdateTagDto> Tags { get; init; }
    public ICollection<UpdateStepDto> Steps { get; init; }
    public ICollection<UpdateIngredientDto> Ingredients { get; init; }
}
