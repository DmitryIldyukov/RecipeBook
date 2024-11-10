using Application.Common.CQRS.Command;
using Application.Common.CQRS.Query;
using Application.Common.Result;
using Application.UseCases.Users.Commands.Create;
using Application.UseCases.Users.Commands.Update;
using Application.UseCases.Users.Dtos;
using Application.UseCases.Users.Queries.GetById;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.Users;

public static class UserBindings
{
    public static void AddUserBindings( this IServiceCollection services )
    {
        services.AddScoped<IValidator<CreateUserCommand>, CreateUserCommandValidator>();
        services.AddScoped<IValidator<UpdateUserCommand>, UpdateUserCommandValidator>();
        services.AddScoped<IValidator<GetUserByIdQuery>, GetUserByIdQueryValidator>();

        services.AddScoped<ICommandHandler<CreateUserCommand, Result>, CreateUserCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateUserCommand, Result>, UpdateUserCommandHandler>();

        services.AddScoped<IQueryHandler<GetUserByIdQuery, ResultT<GetUserQueryDto>>, GetUserByIdQueryHandler>();
    }
}
