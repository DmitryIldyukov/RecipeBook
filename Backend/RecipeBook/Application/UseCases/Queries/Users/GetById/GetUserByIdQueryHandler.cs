using Application.Common.CQRS.Query;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.UseCases.Queries.Users.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Queries.Users.GetById;

public class GetUserByIdQueryHandler(
    IUserRepository userRepository, IUnitOfWork unitOfWork, IValidator<GetUserByIdQuery> validator, IMapper mapper
) : IQueryHandler<GetUserByIdQuery, GetUserQueryDto>
{
    public async Task<GetUserQueryDto> Handle( GetUserByIdQuery query )
    {
        ValidationResult validationResult = await validator.ValidateAsync( query );
        if ( !validationResult.IsValid )
        {
            throw new ValidationException( validationResult.Errors );
        }

        User user = await userRepository.GetById( query.Id );
        if ( user is null )
        {
            throw new NotFoundException( $"Пользователь с id {query.Id} не найден." );
        }

        await unitOfWork.Commit();

        GetUserQueryDto response = mapper.Map<GetUserQueryDto>( user );

        return response;
    }
}
