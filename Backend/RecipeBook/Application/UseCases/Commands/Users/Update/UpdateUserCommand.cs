namespace Application.UseCases.Commands.Users.Update;

public class UpdateUserCommand
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Login { get; init; }
    public string Password { get; init; }
    public string Information { get; init; }
}

