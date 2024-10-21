namespace Application.UseCases.Queries.Users.Dtos;

public class GetUserQueryDto
{
    public int Id { get; init; }
    public string Login { get; init; }
    public string Email { get; init; }
    public string Information { get; init; }
}
