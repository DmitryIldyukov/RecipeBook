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
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return ResultT<TokenInfoDto>.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        User user = await userRepository.GetByLogin( command.Login );

        if ( !passwordHasher.VerifyPassword( command.Password, user.Password ) )
        {
            return ResultT<TokenInfoDto>.Fail( errorMessage );
        }

        TokenInfoDto response = await GenerateTokenResponseAsync( user.Id );

        return ResultT<TokenInfoDto>.Success( response, "Успешный вход." );
    }

    private async Task<TokenInfoDto> GenerateTokenResponseAsync( int userId )
    {
        string token = jwtProvider.GenerateAccessToken( userId );

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
