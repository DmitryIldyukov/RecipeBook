using Application.Common.CQRS.Query;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.UseCases.Queries.Tags.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Queries.Tags.GetByName;

public class GetTagByNameQueryHandler(
    ITagRepository tagRepository, IUnitOfWork unitOfWork, IValidator<GetTagByNameQuery> validator, IMapper mapper
) : IQueryHandler<GetTagByNameQuery, GetTagDto>
{
    public async Task<GetTagDto> Handle( GetTagByNameQuery query )
    {
        ValidationResult validationResult = await validator.ValidateAsync( query );
        if ( !validationResult.IsValid )
        {
            throw new ValidationException( validationResult.Errors );
        }

        Tag tag = await tagRepository.GetByName( query.Tag )
            ?? throw new NotFoundException( "Тэг не найден." );

        return mapper.Map<GetTagDto>( tag );
    }
}
