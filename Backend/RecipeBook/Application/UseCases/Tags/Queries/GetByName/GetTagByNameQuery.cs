namespace Application.UseCases.Tags.Queries.GetByName;

public record GetTagByNameQuery
{
    public string Tag { get; init; }
}
