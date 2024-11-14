using Application.Common.CQRS.Command;
using Application.Common.JwtProvider;
using Application.Common.PasswordHasher;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.UseCases.Users.Dtos;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Users.Commands.Login;

public class LoginUserCommandHandler(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IUnitOfWork unitOfWork,
    IValidator<LoginUserCommand> validator,
    IPasswordHasher passwordHasher,
    IJwtProvider jwtProvider
) : ICommandHandler<LoginUserCommand, ResultT<TokenInfoDto>>
{
    private const string errorMessage = "Неверный логин или пароль.";

    public async Task<ResultT<TokenInfoDto>> Handle( LoginUserCommand command )
    {
        ResultT<TokenInfoDto> validationResult = await ValidateCommandAsync( command );
        if ( !validationResult.IsSuccess )
        {
            return validationResult;
        }

        User user = await userRepository.GetByLogin( command.Login );
        if ( user is null )
        {
            return ResultT<TokenInfoDto>.Fail( errorMessage );
        }

        if ( !passwordHasher.VerifyPassword( command.Password, user.Password ) )
        {
            return ResultT<TokenInfoDto>.Fail( errorMessage );
        }

        TokenInfoDto response = await GenerateTokenResponseAsync( user.Id );

        return ResultT<TokenInfoDto>.Success( response, "Успешный вход." );
    }

    private async Task<ResultT<TokenInfoDto>> ValidateCommandAsync( LoginUserCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return ResultT<TokenInfoDto>.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        return ResultT<TokenInfoDto>.Success( null );
    }

    private async Task<TokenInfoDto> GenerateTokenResponseAsync( int userId )
    {
        string token = jwtProvider.GenerateToken( userId );

        RefreshToken existingRefreshToken = await refreshTokenRepository.GetByUserId( userId );
        if ( existingRefreshToken is not null )
        {
            refreshTokenRepository.Delete( existingRefreshToken );
        }

        RefreshToken refreshToken = jwtProvider.GenerateRefreshToken( userId );

        await refreshTokenRepository.Create( refreshToken );

        await unitOfWork.Commit();

        TokenInfoDto response = new TokenInfoDto()
        {
            AccessToken = token,
            RefreshToken = refreshToken.Token,
        };

        return response;
    }
}
