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
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return Result.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        User user = await userRepository.GetById( command.UserId );

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
}
