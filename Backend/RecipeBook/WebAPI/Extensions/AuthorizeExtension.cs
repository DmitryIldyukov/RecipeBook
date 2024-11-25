using System.Text;
using Infrastructure.JwtProviders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace WebAPI.Extensions;

public static class AuthorizeExtension
{
    public static void AddApiAuthentication(
        this IServiceCollection services,
        IConfiguration configuration )
    {
        JwtOptions jwtOptions = configuration.GetSection( nameof( JwtOptions ) ).Get<JwtOptions>();

        services.AddAuthentication( JwtBearerDefaults.AuthenticationScheme )
            .AddJwtBearer( JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes( jwtOptions!.SecretKey ) ),
                    ClockSkew = TimeSpan.Zero
                };
            } );

        services.AddAuthorization();
    }
}
