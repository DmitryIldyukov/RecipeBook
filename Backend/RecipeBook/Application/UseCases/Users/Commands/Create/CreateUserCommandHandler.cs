using Application.Common.CQRS.Command;
using Application.Common.PasswordHasher;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Users.Commands.Create;

public class CreateUserCommandHandler(
    IUserRepository userRepository, IUnitOfWork unitOfWork, IValidator<CreateUserCommand> validator, IPasswordHasher hasher
) : ICommandHandler<CreateUserCommand, Result>
{
    public async Task<Result> Handle( CreateUserCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return Result.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        string hashedPassword = hasher.HashPassword( command.Password );

        User user = new( command.Name, command.Login, hashedPassword );

        await userRepository.Create( user );

        await unitOfWork.Commit();

        return Result.Success( "Пользователь успешно добавлен." );
    }
}
