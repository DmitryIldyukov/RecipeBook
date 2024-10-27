using System.Reflection;
using Application.Common.CQRS.Command;
using Application.Common.CQRS.Query;
using Application.Common.FileHelper;
using Application.UseCases.Commands.Ingredients;
using Application.UseCases.Commands.Recipes.Create;
using Application.UseCases.Commands.Steps.Create;
using Application.UseCases.Commands.Tags.Create;
using Application.UseCases.Commands.Users.Create;
using Application.UseCases.Commands.Users.Update;
using Application.UseCases.Queries.Recipes.Dtos;
using Application.UseCases.Queries.Recipes.GetRecipeImage;
using Application.UseCases.Queries.Tags.Dtos;
using Application.UseCases.Queries.Tags.GetAll;
using Application.UseCases.Queries.Tags.GetByName;
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
        services.AddScoped<IValidator<CreateTagCommand>, CreateTagCommandValidator>();
        services.AddScoped<IValidator<CreateIngredientCommand>, CreateIngredientCommandValidator>();
        services.AddScoped<IValidator<CreateStepCommand>, CreateStepCommandValidator>();
        services.AddScoped<IValidator<UpdateUserCommand>, UpdateUserCommandValidator>();

        services.AddScoped<ICommandHandler<CreateRecipeCommand>, CreateRecipeCommandHandler>();
        services.AddScoped<ICommandHandler<CreateUserCommand>, CreateUserCommandHandler>();
        services.AddScoped<ICommandHandler<CreateTagCommand, Tag>, CreateTagCommandHandler>();
        services.AddScoped<ICommandHandler<CreateIngredientCommand, Ingredient>, CreateIngredientCommandHandler>();
        services.AddScoped<ICommandHandler<CreateStepCommand, Step>, CreateStepCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateUserCommand>, UpdateUserCommandHandler>();

        services.AddScoped<IValidator<GetUserByIdQuery>, GetUserByIdQueryValidator>();
        services.AddScoped<IValidator<GetTagByNameQuery>, GetTagByNameQueryValidator>();
        services.AddScoped<IValidator<GetAllTagsQuery>, GetAllTagsQueryValidator>();
        services.AddScoped<IValidator<GetRecipeImageQuery>, GetRecipeImageQueryValidator>();

        services.AddScoped<IQueryHandler<GetUserByIdQuery, GetUserQueryDto>, GetUserByIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetTagByNameQuery, GetTagDto>, GetTagByNameQueryHandler>();
        services.AddScoped<IQueryHandler<GetAllTagsQuery, IReadOnlyList<GetTagDto>>, GetAllTagsQueryHandler>();
        services.AddScoped<IQueryHandler<GetRecipeImageQuery, GetImageQueryDto>, GetRecipeImageQueryHandler>();

        return services;
    }
}
