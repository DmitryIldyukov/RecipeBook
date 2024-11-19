using Domain.Entities;

namespace Application.UseCases.Tags.Commands.Delete;

public record DeleteTagsCommand
{
    public int RecipeId { get; init; }
    public ICollection<Tag> Tags { get; init; }
}
