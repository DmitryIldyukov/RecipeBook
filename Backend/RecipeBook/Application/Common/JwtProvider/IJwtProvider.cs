using Domain.Entities;

namespace Application.Common.JwtProvider;

public interface IJwtProvider
{
    string GenerateAccessToken( int userId );
    RefreshToken GenerateRefreshToken( int userId );
}
