using Domain.Entities;

namespace Application.Common.JwtProvider;

public interface IJwtProvider
{
    string GenerateToken( User user );
}
