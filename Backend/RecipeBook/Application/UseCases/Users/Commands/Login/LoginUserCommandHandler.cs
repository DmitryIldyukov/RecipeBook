using Application.Common.CQRS.Command;
using Application.Common.JwtProvider;
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
    IPasswordHasher passwordHasher,
    IJwtProvider jwtProvider
) : ICommandHandler<LoginUserCommand, ResultT<string>>
{
    private const string errorMessage = "Неверный логин или пароль.";

    public async Task<ResultT<string>> Handle( LoginUserCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return ResultT<string>.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        User user = await userRepository.GetByLogin( command.Login );
        if ( user == null )
        {
            return ResultT<string>.Fail( errorMessage );
        }

        if ( !passwordHasher.VerifyPassword( command.Password, user.Password ) )
        {
            return ResultT<string>.Fail( errorMessage );
        }

        string token = jwtProvider.GenerateToken( user );

        return ResultT<string>.Success( token, "Успешный вход." );
    }
}
