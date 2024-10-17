namespace Application.UseCases.Commands.Users.Create;

public class CreateUserCommand
{
    public string Name { get; init; }
    public string Login { get; init; }
    public string Password { get; init; }
}
