using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.UseCases.Steps.Commands.Create;
using Application.UseCases.Steps.Commands.UpdateRecipeSteps;
using Application.UseCases.Steps.Commands.UpdateStep;
using Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.Steps;

public static class StepBindings
{
    public static void AddStepBindings( this IServiceCollection services )
    {
        services.AddScoped<IValidator<CreateStepCommand>, CreateStepCommandValidator>();
        services.AddScoped<IValidator<UpdateStepCommand>, UpdateStepCommandValidator>();
        services.AddScoped<IValidator<UpdateRecipeStepsCommand>, UpdateRecipeStepsCommandValidator>();

        services.AddScoped<ICommandHandler<CreateStepCommand, ResultT<Step>>, CreateStepCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateStepCommand, Result>, UpdateStepCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateRecipeStepsCommand, Result>, UpdateRecipeStepsCommandHandler>();
    }
}
