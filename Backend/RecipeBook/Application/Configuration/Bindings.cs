using Application.Common.CQRS.Command;
using Application.UseCases.Commands.Recipe.Create;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Configuration;

public static class Bindings
{
    public static IServiceCollection AddApplication( this IServiceCollection services )
    {
        services.AddTransient<IValidator<CreateRecipeCommand>, CreateRecipeCommandValidator>();

        services.AddScoped<ICommandHandler<CreateRecipeCommand, int>, CreateRecipeCommandHandler>();

        return services;
    }
}
