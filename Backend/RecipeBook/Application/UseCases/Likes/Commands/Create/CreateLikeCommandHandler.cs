using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Likes.Commands.Create;

public class CreateLikeCommandHandler(
    ILikeRepository likeRepository,
    IValidator<CreateLikeCommand> validator,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ICommandHandler<CreateLikeCommand, Result>
{
    public async Task<Result> Handle( CreateLikeCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return Result.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        Like like = mapper.Map<Like>( command );

        await likeRepository.Create( like );
        await unitOfWork.Commit();

        return Result.Success();
    }
}
