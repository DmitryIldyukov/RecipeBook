using Application.Common.CQRS.Command;
using Application.Common.JwtProvider;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.UseCases.Users.Dtos;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.RefreshTokens.Commands.Refresh;

public class RefreshTokenCommandHandler(
    IRefreshTokenRepository refreshTokenRepository,
    IUnitOfWork unitOfWork,
    IJwtProvider jwtProvider,
    IValidator<RefreshTokenCommand> validator
) : ICommandHandler<RefreshTokenCommand, ResultT<TokenInfoDto>>
{
    private const string errorMessage = "Токен недействителен.";

    public async Task<ResultT<TokenInfoDto>> Handle( RefreshTokenCommand command )
    {
        ResultT<TokenInfoDto> validationResult = await ValidateCommandAsync( command );
        if ( !validationResult.IsSuccess )
        {
            return validationResult;
        }

        RefreshToken refreshToken = await refreshTokenRepository.GetByToken( command.RefreshToken );
        if ( refreshToken is null )
        {
            return ResultT<TokenInfoDto>.Fail( errorMessage );
        }

        refreshTokenRepository.Delete( refreshToken );

        if ( !IsValidRefreshToken( refreshToken ) )
        {
            return ResultT<TokenInfoDto>.Fail( errorMessage );
        }

        return await GenerateAndSaveNewTokensAsync( refreshToken );
    }

    private async Task<ResultT<TokenInfoDto>> ValidateCommandAsync( RefreshTokenCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return ResultT<TokenInfoDto>.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        return ResultT<TokenInfoDto>.Success( null );
    }

    private bool IsValidRefreshToken( RefreshToken refreshToken )
    {
        return refreshToken.ExpirationDate > DateTime.UtcNow;
    }

    private async Task<ResultT<TokenInfoDto>> GenerateAndSaveNewTokensAsync( RefreshToken oldRefreshToken )
    {
        int userId = oldRefreshToken.UserId;

        string newAccessToken = jwtProvider.GenerateToken( userId );
        RefreshToken newRefreshToken = jwtProvider.GenerateRefreshToken( userId );

        await refreshTokenRepository.Create( newRefreshToken );
        await unitOfWork.Commit();

        TokenInfoDto response = new TokenInfoDto()
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken.Token,
        };

        return ResultT<TokenInfoDto>.Success( response, "Успешный вход." );
    }
}
