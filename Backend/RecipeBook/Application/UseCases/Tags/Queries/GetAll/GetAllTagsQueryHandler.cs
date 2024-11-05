using Application.Common.CQRS.Query;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using Application.UseCases.Tags.Dtos;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Tags.Queries.GetAll;

public class GetAllTagsQueryHandler(
    ITagRepository tagRepository, IValidator<GetAllTagsQuery> validator, IMapper mapper
) : IQueryHandler<GetAllTagsQuery, ResultT<IReadOnlyList<GetTagDto>>>
{
    public async Task<ResultT<IReadOnlyList<GetTagDto>>> Handle( GetAllTagsQuery query )
    {
        ValidationResult validationResult = await validator.ValidateAsync( query );
        if ( !validationResult.IsValid )
        {
            return ResultT<IReadOnlyList<GetTagDto>>.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        IQueryable<Tag> tags = tagRepository.GetAll();

        return ResultT<IReadOnlyList<GetTagDto>>.Success( await mapper.ProjectTo<GetTagDto>( tags ).ToListAsync(), "Тэги успешно получены." );
    }
}
