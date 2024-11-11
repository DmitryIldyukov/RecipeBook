using Application.Common.CQRS.Command;
using Application.Common.CQRS.Query;
using Application.Common.Result;
using Application.UseCases.Tags.Commands.Create;
using Application.UseCases.Tags.Commands.Delete;
using Application.UseCases.Tags.Commands.UpdateRecipeTags;
using Application.UseCases.Tags.Dtos;
using Application.UseCases.Tags.Queries.GetAll;
using Application.UseCases.Tags.Queries.GetByName;
using Application.UseCases.Tags.Queries.GetPopular;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.Tags;

public static class TagBindings
{
    public static void AddTagBindings( this IServiceCollection services )
    {
        services.AddScoped<IValidator<CreateTagCommand>, CreateTagCommandValidator>();
        services.AddScoped<IValidator<UpdateRecipeTagsCommand>, UpdateRecipeTagsCommandValidator>();
        services.AddScoped<IValidator<DeleteTagsCommand>, DeleteTagsCommandValidator>();
        services.AddScoped<IValidator<GetTagByNameQuery>, GetTagByNameQueryValidator>();
        services.AddScoped<IValidator<GetPopularTagsQuery>, GetPopularTagsQueryValidator>();

        services.AddScoped<ICommandHandler<CreateTagCommand, Result>, CreateTagCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateRecipeTagsCommand, Result>, UpdateRecipeTagsCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteTagsCommand, Result>, DeleteTagsCommandHandler>();

        services.AddScoped<IQueryHandler<GetTagByNameQuery, ResultT<GetTagDto>>, GetTagByNameQueryHandler>();
        services.AddScoped<IQueryHandler<GetAllTagsQuery, ResultT<IReadOnlyList<GetTagDto>>>, GetAllTagsQueryHandler>();
        services.AddScoped<IQueryHandler<GetPopularTagsQuery, ResultT<IReadOnlyList<GetTagDto>>>, GetPopularTagsQueryHandler>();
    }
}
