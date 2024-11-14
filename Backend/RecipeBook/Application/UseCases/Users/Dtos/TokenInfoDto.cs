namespace Application.UseCases.Users.Dtos;

public class TokenInfoDto
{
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
}
