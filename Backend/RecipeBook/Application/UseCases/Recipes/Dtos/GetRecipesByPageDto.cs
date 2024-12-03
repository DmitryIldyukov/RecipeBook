namespace Application.UseCases.Recipes.Dtos;

public class GetRecipesByPageDto
{
    public bool HasTakeMoreRecipes { get; init; }
    public IReadOnlyList<GetRecipeQueryDto> Recipes { get; init; }
}
