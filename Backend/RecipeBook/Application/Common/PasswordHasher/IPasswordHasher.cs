namespace Application.Common.PasswordHasher;

public interface IPasswordHasher
{
    public string HashPassword( string password );
}
