namespace Application.UseCases.Users.Commands.Update;

public class UpdateUserCommand
{
    public int UserId { get; init; }
    public string Name { get; init; }
    public string Login { get; init; }
    public string Password { get; init; }
    public string Information { get; init; }
}

