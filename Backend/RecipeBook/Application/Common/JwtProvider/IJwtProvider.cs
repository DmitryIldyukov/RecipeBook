using Domain.Entities;

namespace Application.Common.JwtProvider;

public interface IJwtProvider
{
    string GenerateToken( int userId );
    RefreshToken GenerateRefreshToken( int userId );
}
