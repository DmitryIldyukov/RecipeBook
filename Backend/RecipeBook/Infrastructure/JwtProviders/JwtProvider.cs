using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Common.JwtProvider;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.JwtProviders;

public class JwtProvider( IOptions<JwtOptions> options ) : IJwtProvider
{
    private readonly JwtOptions _options = options.Value;

    public string GenerateAccessToken( int userId )
    {
        Claim[] claims = [ new Claim( nameof( userId ), userId.ToString() ) ];

        SigningCredentials signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey( Encoding.UTF8.GetBytes( _options.SecretKey ) ),
            SecurityAlgorithms.HmacSha256
        );

        JwtSecurityToken token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddMinutes( _options.AccessTokenExpiresMinutes )
        );

        string tokenValue = new JwtSecurityTokenHandler().WriteToken( token );

        return tokenValue;
    }

    public RefreshToken GenerateRefreshToken( int userId )
    {
        string token = Convert.ToBase64String( RandomNumberGenerator.GetBytes( 64 ) );
        DateTime expired = DateTime.UtcNow.AddDays( _options.RefreshTokenExpiresDays );
        RefreshToken refreshToken = new RefreshToken( userId, token, expired );

        return refreshToken;
    }
}
