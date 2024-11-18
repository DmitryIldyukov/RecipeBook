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
        User user = await userRepository.GetByLogin( command.Login );

        ResultT<int> validationResult = await ValidateAsync( command, user );
        if ( !validationResult.IsSuccess )
        {
            return validationResult;
        }

        if ( !passwordHasher.VerifyPassword( command.Password, user.Password ) )
        {
            return ResultT<int>.Fail( errorMessage );
        }

        return ResultT<int>.Success( user.Id, "Успешный вход." );
    }

    private async Task<ResultT<int>> ValidateAsync( LoginUserCommand command, User user )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return ResultT<int>.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        if ( user == null )
        {
            return ResultT<int>.Fail( errorMessage );
        }

        return ResultT<int>.Success( 0 );
    }
}
