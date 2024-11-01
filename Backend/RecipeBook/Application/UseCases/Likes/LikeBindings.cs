using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.UseCases.Likes.Commands.Create;
using Application.UseCases.Likes.Commands.Delete;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.Likes;

public static class LikeBindings
{
    public static void AddLikeBidings( this IServiceCollection services )
    {
        services.AddScoped<IValidator<CreateLikeCommand>, CreateLikeCommandValidator>();
        services.AddScoped<IValidator<DeleteLikeCommand>, DeleteLikeCommandValidator>();

        services.AddScoped<ICommandHandler<CreateLikeCommand, Result>, CreateLikeCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteLikeCommand, Result>, DeleteLikeCommandHandler>();
    }
}
