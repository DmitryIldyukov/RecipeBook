using Application.Common.CQRS.Command;
using Application.Common.PasswordHasher;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Commands.Users.Create;

public class CreateUserCommandHandler(
    IUserRepository userRepository, IUnitOfWork unitOfWork, IValidator<CreateUserCommand> validator, IPasswordHasher hasher
) : ICommandHandler<CreateUserCommand>
{
    public async Task Handle( CreateUserCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            throw new ValidationException( validationResult.Errors );
        }

        string hashedPassword = hasher.HashPassword( command.Password );

        User user = new( command.Name, command.Login, hashedPassword );

        await userRepository.Create( user );

        await unitOfWork.Commit();
    }
}
