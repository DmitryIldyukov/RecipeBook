using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Favorites.Commands.Create;

public class CreateFavoriteCommandHandler(
    IFavoriteRepository favoriteRepository,
    IValidator<CreateFavoriteCommand> validator,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ICommandHandler<CreateFavoriteCommand, Result>
{
    public async Task<Result> Handle( CreateFavoriteCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return Result.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        Favorite favorite = mapper.Map<Favorite>( command );

        await favoriteRepository.Create( favorite );
        await unitOfWork.Commit();

        return Result.Success();
    }
}
