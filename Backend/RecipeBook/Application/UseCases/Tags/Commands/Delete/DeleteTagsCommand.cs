using Domain.Entities;

namespace Application.UseCases.Tags.Commands.Delete;

public class DeleteTagsCommand
{
    public int RecipeId { get; init; }
    public ICollection<Tag> Tags { get; init; }
}
