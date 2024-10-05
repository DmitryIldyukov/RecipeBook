using Application.UseCases.Commands.Recipe.Create;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Configuration;

public static class Bindings
{
    public static IServiceCollection AddApplication( this IServiceCollection services )
    {
        services.AddTransient<IValidator<CreateRecipeCommand>, CreateRecipeCommandValidator>();

        services.AddMediatR( typeof( Bindings ).Assembly );

        return services;
    }
}
