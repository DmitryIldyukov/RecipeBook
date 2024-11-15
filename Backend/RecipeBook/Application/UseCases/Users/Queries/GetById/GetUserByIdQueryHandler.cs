using Application.Common.CQRS.Query;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.UseCases.Tags.Dtos;
using Application.UseCases.Tags.Queries.GetByName;
using Application.UseCases.Users.Dtos;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Users.Queries.GetById;

public class GetUserByIdQueryHandler(
    IUserRepository userRepository, IUnitOfWork unitOfWork, IValidator<GetUserByIdQuery> validator, IMapper mapper
) : IQueryHandler<GetUserByIdQuery, ResultT<GetUserQueryDto>>
{
    public async Task<ResultT<GetUserQueryDto>> Handle( GetUserByIdQuery query )
    {
        ResultT<GetUserQueryDto> validationResult = await ValidateCommandAsync( query );
        if ( !validationResult.IsSuccess )
        {
            return validationResult;
        }

        User user = await userRepository.GetById( query.Id );
        if ( user is null )
        {
            return ResultT<GetUserQueryDto>.Fail( $"Пользователь с id {query.Id} не найден." );
        }

        await unitOfWork.Commit();

        GetUserQueryDto response = mapper.Map<GetUserQueryDto>( user );

        return ResultT<GetUserQueryDto>.Success( response, $"Пользователь с id {query.Id} найден." );
    }

    private async Task<ResultT<GetUserQueryDto>> ValidateCommandAsync( GetUserByIdQuery query )
    {
        ValidationResult validationResult = await validator.ValidateAsync( query );
        if ( !validationResult.IsValid )
        {
            return ResultT<GetUserQueryDto>.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        return ResultT<GetUserQueryDto>.Success( null );
    }
}
