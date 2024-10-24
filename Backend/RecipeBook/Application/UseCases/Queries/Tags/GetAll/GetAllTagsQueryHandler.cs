using Application.Common.CQRS.Query;
using Application.Interfaces.Repositories;
using Application.UseCases.Queries.Tags.Dtos;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Queries.Tags.GetAll;

public class GetAllTagsQueryHandler(
    ITagRepository tagRepository, IValidator<GetAllTagsQuery> validator, IMapper mapper
) : IQueryHandler<GetAllTagsQuery, IReadOnlyList<GetTagDto>>
{
    public async Task<IReadOnlyList<GetTagDto>> Handle( GetAllTagsQuery query )
    {
        ValidationResult validationResult = await validator.ValidateAsync( query );
        if ( !validationResult.IsValid )
        {
            throw new ValidationException( validationResult.Errors );
        }

        IQueryable<Tag> tags = await tagRepository.GetAll();

        return await mapper.ProjectTo<GetTagDto>( tags ).ToListAsync();
    }
}
