namespace Application.UseCases.Users.Commands.Login;

public record LoginUserCommand
{
    public string Login { get; init; }
    public string Password { get; init; }
}
