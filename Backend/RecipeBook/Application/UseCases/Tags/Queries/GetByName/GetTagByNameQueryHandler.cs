using Application.Common.CQRS.Query;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using Application.UseCases.Tags.Dtos;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Tags.Queries.GetByName;

public class GetTagByNameQueryHandler(
    ITagRepository tagRepository, IValidator<GetTagByNameQuery> validator, IMapper mapper
) : IQueryHandler<GetTagByNameQuery, ResultT<GetTagDto>>
{
    public async Task<ResultT<GetTagDto>> Handle( GetTagByNameQuery query )
    {
        ValidationResult validationResult = await validator.ValidateAsync( query );
        if ( !validationResult.IsValid )
        {
            return ResultT<GetTagDto>.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        Tag tag = await tagRepository.GetByName( query.Tag );
        if ( tag is null )
        {
            return ResultT<GetTagDto>.Fail( "Тег не найден." );
        }

        return ResultT<GetTagDto>.Success( mapper.Map<GetTagDto>( tag ), $"Тег {tag.Name} найден." );
    }
}
