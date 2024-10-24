using Application.Common.CQRS.Command;
using Application.Common.PasswordHasher;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Commands.Users.Update;

public class UpdateUserCommandHandler(
    IUserRepository userRepository, IUnitOfWork unitOfWork, IValidator<UpdateUserCommand> validator, IPasswordHasher hasher
) : ICommandHandler<UpdateUserCommand>
{
    public async Task Handle( UpdateUserCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            throw new ValidationException( validationResult.Errors );
        }

        User user = await userRepository.GetById( command.Id )
            ?? throw new NotFoundException( $"Пользователь с Id {command.Id} не найден." );

        user.Name = command.Name;
        user.Login = command.Login;
        user.Information = command.Information;
        if ( !string.IsNullOrEmpty( command.Password ) )
        {
            user.Password = hasher.HashPassword( command.Password );
        }

        await unitOfWork.Commit();
    }
}
