using Application.UseCases.Recipes.Dtos;
using Domain.Entities;

namespace Application.UseCases.Tags.Commands.UpdateRecipeTags;

public class UpdateRecipeTagsCommand
{
    public Recipe Recipe { get; init; }
    public ICollection<RecipeTagDto> Tags { get; init; }
}
