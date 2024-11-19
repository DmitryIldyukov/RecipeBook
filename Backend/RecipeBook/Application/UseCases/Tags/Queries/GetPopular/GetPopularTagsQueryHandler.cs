using Application.Common.CQRS.Query;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using Application.UseCases.Tags.Dtos;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Tags.Queries.GetPopular;

public class GetPopularTagsQueryHandler(
    ITagRepository tagRepository,
    IValidator<GetPopularTagsQuery> validator,
    IMapper mapper
) : IQueryHandler<GetPopularTagsQuery, ResultT<IReadOnlyList<GetTagDto>>>
{
    public async Task<ResultT<IReadOnlyList<GetTagDto>>> Handle( GetPopularTagsQuery query )
    {
        ResultT<IReadOnlyList<GetTagDto>> validationResult = await ValidateAsync( query );
        if ( !validationResult.IsSuccess )
        {
            return validationResult;
        }

        IReadOnlyList<Tag> tags = await tagRepository.GetPopularTags( query.Count );

        return ResultT<IReadOnlyList<GetTagDto>>.Success( mapper.Map<IReadOnlyList<GetTagDto>>( tags ), "Популярные теги успешно получены." );
    }

    private async Task<ResultT<IReadOnlyList<GetTagDto>>> ValidateAsync( GetPopularTagsQuery query )
    {
        ValidationResult validationResult = await validator.ValidateAsync( query );
        if ( !validationResult.IsValid )
        {
            return ResultT<IReadOnlyList<GetTagDto>>.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        return ResultT<IReadOnlyList<GetTagDto>>.Success( null );
    }
}
