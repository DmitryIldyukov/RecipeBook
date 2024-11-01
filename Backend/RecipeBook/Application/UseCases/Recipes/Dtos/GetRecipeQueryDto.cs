using Application.UseCases.Ingredients.Dtos;
using Application.UseCases.Steps.Dtos;
using Application.UseCases.Tags.Dtos;

namespace Application.UseCases.Recipes.Dtos;

public class GetRecipeQueryDto
{
    public int RecipeId { get; init; }
    public int AuthorId { get; init; }
    public string AuthorLogin { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public int CookTime { get; init; }
    public int PortionCount { get; init; }
    public int LikesCount { get; init; }
    public int FavoritesCount { get; init; }
    public ICollection<GetTagDto> Tags { get; init; }
    public ICollection<GetStepDto> Steps { get; init; }
    public ICollection<GetIngredientDto> Ingredients { get; init; }
}
