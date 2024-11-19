using Application.Common.CQRS.Command;
using Application.Common.PasswordHasher;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Users.Commands.Login;

public class LoginUserCommandHandler(
    IUserRepository userRepository,
    IValidator<LoginUserCommand> validator,
    IPasswordHasher passwordHasher
) : ICommandHandler<LoginUserCommand, ResultT<int>>
{
    private const string errorMessage = "Неверный логин или пароль.";

    public async Task<ResultT<int>> Handle( LoginUserCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return ResultT<int>.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        User user = await userRepository.GetByLogin( command.Login );

        if ( !passwordHasher.VerifyPassword( command.Password, user.Password ) )
        {
            return ResultT<int>.Fail( errorMessage );
        }

        return ResultT<int>.Success( user.Id, "Успешный вход." );
    }
}
