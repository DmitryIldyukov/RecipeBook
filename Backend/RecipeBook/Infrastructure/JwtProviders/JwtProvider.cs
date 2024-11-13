using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Common.JwtProvider;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.JwtProviders;

public class JwtProvider( IOptions<JwtOptions> options ) : IJwtProvider
{
    private readonly JwtOptions _options = options.Value;

    public string GenerateToken( User user )
    {
        Claim[] claims = [ new Claim( "userId", user.Id.ToString() ) ];

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
}
