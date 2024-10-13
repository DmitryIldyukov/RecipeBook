using Application.Common.CQRS.Command;
using Application.Common.PasswordHasher;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.UseCases.Commands.User.Create;

public class CreateUserCommandHandler( IUserRepository repo, IUnitOfWork unitOfWork, IValidator<CreateUserCommand> validator, IPasswordHasher hasher )
    : ICommandHandler<CreateUserCommand>
{
    public async Task Handle( CreateUserCommand command )
    {
        var validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            throw new ValidationException( validationResult.Errors );
        }

        string hashedPassword = hasher.HashPassword( command.Password );

        Domain.Entities.User user = new Domain.Entities.User( command.Name, command.Login, hashedPassword );

        await repo.Create( user );

        await unitOfWork.Commit();
    }
}
