namespace Application.UseCases.Users.Commands.Create;

public record CreateUserCommand
{
    public string Name { get; init; }
    public string Login { get; init; }
    public string Password { get; init; }
}
