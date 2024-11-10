using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.UseCases.Steps.Commands.Create;
using Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.Steps;

public static class StepBindings
{
    public static void AddStepBindings( this IServiceCollection services )
    {
        services.AddScoped<IValidator<CreateStepCommand>, CreateStepCommandValidator>();

        services.AddScoped<ICommandHandler<CreateStepCommand, Result>, CreateStepCommandHandler>();
    }
}
