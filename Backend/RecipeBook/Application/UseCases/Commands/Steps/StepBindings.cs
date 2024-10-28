using Application.Common.CQRS.Command;
using Application.UseCases.Commands.Steps.Create;
using Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.Commands.Steps;

public static class StepBindings
{
    public static void AddStepBindings( this IServiceCollection services )
    {
        services.AddScoped<IValidator<CreateStepCommand>, CreateStepCommandValidator>();

        services.AddScoped<ICommandHandler<CreateStepCommand, Step>, CreateStepCommandHandler>();
    }
}
