namespace Application.UseCases.Users.Dtos;

public class GetUserQueryDto
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Login { get; init; }
    public string Information { get; init; }
}
