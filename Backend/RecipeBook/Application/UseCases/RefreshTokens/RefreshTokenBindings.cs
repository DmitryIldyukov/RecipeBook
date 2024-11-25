using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.UseCases.RefreshTokens.Commands.Refresh;
using Application.UseCases.Users.Dtos;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.RefreshTokens;

public static class RefreshTokenBindings
{
    public static void AddRefreshTokenBindings( this IServiceCollection services )
    {
        services.AddScoped<IValidator<RefreshTokenCommand>, RefreshTokenCommandValidator>();

        services.AddScoped<ICommandHandler<RefreshTokenCommand, ResultT<TokenInfoDto>>, RefreshTokenCommandHandler>();
    }
}