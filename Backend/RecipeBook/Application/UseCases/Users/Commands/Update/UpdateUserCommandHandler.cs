using Application.Common.CQRS.Command;
using Application.Common.PasswordHasher;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Users.Commands.Update;

public class UpdateUserCommandHandler(
    IUserRepository userRepository, IUnitOfWork unitOfWork, IValidator<UpdateUserCommand> validator, IPasswordHasher hasher
) : ICommandHandler<UpdateUserCommand, Result>
{
    public async Task<Result> Handle( UpdateUserCommand command )
    {
        User user = await userRepository.GetById( command.UserId );

        Result validationResult = await ValidateAsync( command, user );
        if ( !validationResult.IsSuccess )
        {
            return validationResult;
        }

        user.Name = command.Name;
        user.Login = command.Login;
        user.Information = command.Information;
        if ( !string.IsNullOrEmpty( command.Password ) )
        {
            user.Password = hasher.HashPassword( command.Password );
        }

        await unitOfWork.Commit();

        return Result.Success( "Данные пользователя успешно изменены." );
    }

    private async Task<Result> ValidateAsync( UpdateUserCommand command, User user )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return Result.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        if ( user is null )
        {
            return Result.Fail( $"Пользователь с Id {command.UserId} не найден." );
        }

        Result validationUserOwnershipResult = ValidateUserOwnership( user.Id, command.UserId );
        if ( !validationUserOwnershipResult.IsSuccess )
        {
            return validationUserOwnershipResult;
        }

        return Result.Success();
    }

    private Result ValidateUserOwnership( int userId, int currentUserId )
    {
        if ( userId != currentUserId )
        {
            return Result.Fail( "Невозможно изменить данные другого пользователя." );
        }

        return Result.Success();
    }
}
