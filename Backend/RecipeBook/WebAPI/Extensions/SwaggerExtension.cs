using Microsoft.OpenApi.Models;
using System.Reflection;

namespace WebAPI.Extensions;

public static class SwaggerExtensions
{
    private const string SecuritySchemeName = "Bearer";
    private const string AuthorizationHeaderName = "Authorization";

    public static void AddSwagger( this IServiceCollection service )
    {
        service.AddSwaggerGen( options =>
        {
            options.AddSecurityDefinition( SecuritySchemeName, new OpenApiSecurityScheme()
            {
                Name = AuthorizationHeaderName,
                Type = SecuritySchemeType.ApiKey,
                Scheme = SecuritySchemeName,
                BearerFormat = "JWT",
                In = ParameterLocation.Header
            } );

            options.AddSecurityRequirement( new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = SecuritySchemeName
                        }
                    },
                    Array.Empty<string>()
                }
            } );
        } );
    }
}