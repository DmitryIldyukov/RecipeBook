using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.UseCases.Favorites.Commands.Create;
using Application.UseCases.Favorites.Commands.Delete;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.Favorites;

public static class FavoriteBindings
{
    public static void AddFavoriteBindings( this IServiceCollection services )
    {
        services.AddScoped<IValidator<CreateFavoriteCommand>, CreateFavoriteCommandValidator>();
        services.AddScoped<IValidator<DeleteFavoriteCommand>, DeleteFavoriteCommandValidator>();

        services.AddScoped<ICommandHandler<CreateFavoriteCommand, Result>, CreateFavoriteCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteFavoriteCommand, Result>, DeleteFavoriteCommandHandler>();
    }
}
