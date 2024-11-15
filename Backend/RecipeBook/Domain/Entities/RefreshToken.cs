namespace Domain.Entities;

public class RefreshToken : Entity
{
    public int UserId { get; init; }
    public User User { get; init; }
    public string Token { get; init; }
    public DateTime ExpirationDate { get; init; }

    public RefreshToken( int userId, string token, DateTime expirationDate )
    {
        UserId = userId;
        Token = token;
        ExpirationDate = expirationDate;
    }
}
