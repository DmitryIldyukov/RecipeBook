namespace Application.UseCases.RefreshTokens.Commands.Refresh;

public record RefreshTokenCommand
{
    public string RefreshToken { get; init; }
}
