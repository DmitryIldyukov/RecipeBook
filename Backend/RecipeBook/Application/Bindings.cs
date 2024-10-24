using System.Reflection;
using Application.Common.CQRS.Command;
using Application.Common.CQRS.Query;
using Application.UseCases.Commands.Recipe.Create;
using Application.UseCases.Commands.Users.Create;
using Application.UseCases.Commands.Users.Update;
using Application.UseCases.Queries.Users.Dtos;
using Application.UseCases.Queries.Users.GetById;
using Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class Bindings
{
    public static IServiceCollection AddApplication( this IServiceCollection services )
    {
        services.AddAutoMapper( Assembly.GetExecutingAssembly() );

        services.AddScoped<IValidator<CreateRecipeCommand>, CreateRecipeCommandValidator>();
        services.AddScoped<IValidator<CreateUserCommand>, CreateUserCommandValidator>();
        services.AddScoped<IValidator<UpdateUserCommand>, UpdateUserCommandValidator>();

        services.AddScoped<ICommandHandler<CreateRecipeCommand>, CreateRecipeCommandHandler>();
        services.AddScoped<ICommandHandler<CreateUserCommand>, CreateUserCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateUserCommand>, UpdateUserCommandHandler>();

        services.AddScoped<IValidator<GetUserByIdQuery>, GetUserByIdQueryValidatior>();
        services.AddScoped<IValidator<GetUserByIdQuery>, GetUserByIdQueryValidator>();

        services.AddScoped<IQueryHandler<GetUserByIdQuery, GetUserQueryDto>, GetUserByIdQueryHandler>();

        return services;
    }
}
