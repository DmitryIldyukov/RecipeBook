using Application.Common.PasswordHasher;

namespace Infrastructure.PasswordHasher;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword( string password )
    {
        return BCrypt.Net.BCrypt.HashPassword( password );
    }
}
