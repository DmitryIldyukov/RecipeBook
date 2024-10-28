using Application.Common.CQRS.Command;
using Application.Common.CQRS.Query;
using Application.UseCases.Commands.Tags.Create;
using Application.UseCases.Queries.Tags.Dtos;
using Application.UseCases.Queries.Tags.GetAll;
using Application.UseCases.Queries.Tags.GetByName;
using Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.Commands.Tags;

public static class TagBindings
{
    public static void AddTagBindings( this IServiceCollection services )
    {
        services.AddScoped<IValidator<CreateTagCommand>, CreateTagCommandValidator>();
        services.AddScoped<IValidator<GetAllTagsQuery>, GetAllTagsQueryValidator>();
        services.AddScoped<IValidator<GetTagByNameQuery>, GetTagByNameQueryValidator>();

        services.AddScoped<ICommandHandler<CreateTagCommand, Tag>, CreateTagCommandHandler>();

        services.AddScoped<IQueryHandler<GetTagByNameQuery, GetTagDto>, GetTagByNameQueryHandler>();
        services.AddScoped<IQueryHandler<GetAllTagsQuery, IReadOnlyList<GetTagDto>>, GetAllTagsQueryHandler>();
    }
}
