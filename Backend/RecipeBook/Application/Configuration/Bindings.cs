using Application.Common.CQRS.Command;
using Application.UseCases.Commands.Recipe.Create;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Configuration;

public static class Bindings
{
    public static IServiceCollection AddApplication( this IServiceCollection services )
    {
        services.AddScoped<IValidator<CreateRecipeCommand>, CreateRecipeCommandValidator>();

        services.AddScoped<ICommandHandler<CreateRecipeCommand>, CreateRecipeCommandHandler>();

        return services;
    }
}
