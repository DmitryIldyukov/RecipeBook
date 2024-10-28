using Application.Common.CQRS.Command;
using Application.Common.CQRS.Query;
using Application.UseCases.Commands.Users.Create;
using Application.UseCases.Commands.Users.Update;
using Application.UseCases.Queries.Users.Dtos;
using Application.UseCases.Queries.Users.GetById;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.Commands.Users;

public static class UserBindings
{
    public static void AddUserBindings( this IServiceCollection services )
    {
        services.AddScoped<IValidator<CreateUserCommand>, CreateUserCommandValidator>();
        services.AddScoped<IValidator<UpdateUserCommand>, UpdateUserCommandValidator>();
        services.AddScoped<IValidator<GetUserByIdQuery>, GetUserByIdQueryValidator>();

        services.AddScoped<ICommandHandler<CreateUserCommand>, CreateUserCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateUserCommand>, UpdateUserCommandHandler>();

        services.AddScoped<IQueryHandler<GetUserByIdQuery, GetUserQueryDto>, GetUserByIdQueryHandler>();
    }
}
