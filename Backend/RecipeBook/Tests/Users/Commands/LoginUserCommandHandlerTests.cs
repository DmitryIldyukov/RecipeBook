using Application.Common.JwtProvider;
using Application.Common.PasswordHasher;
using Application.Interfaces.Repositories;
using Application.Interfaces;
using Application.UseCases.Users.Commands.Login;
using Domain.Entities;
using Moq;
using Application.Common.Result;
using Application.UseCases.Users.Dtos;
using System.Linq.Expressions;

namespace Tests.Users.Commands;

public class LoginUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<IJwtProvider> _jwtProviderMock;
    private readonly LoginUserCommandValidator _validator;
    private readonly LoginUserCommandHandler _handler;

    public LoginUserCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _jwtProviderMock = new Mock<IJwtProvider>();
        _validator = new LoginUserCommandValidator( _userRepositoryMock.Object );
        _handler = new LoginUserCommandHandler(
            _userRepositoryMock.Object,
            _refreshTokenRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _validator,
            _passwordHasherMock.Object,
            _jwtProviderMock.Object
        );
    }

    [Fact]
    public async Task Handle_ValidCommandAndLoginData_ReturnsToken()
    {
        // Arrange
        LoginUserCommand command = new LoginUserCommand { Login = "user123", Password = "ValidPass123" };
        User user = new User( "User Name", "user123", "hashedPassword" );

        _userRepositoryMock.Setup( repo => repo.GetByLogin( command.Login ) )
            .ReturnsAsync( user );

        _passwordHasherMock.Setup( hasher => hasher.VerifyPassword( command.Password, user.Password ) )
            .Returns( true );

        _jwtProviderMock.Setup( provider => provider.GenerateToken( user.Id ) )
            .Returns( "jwtToken" );

        _jwtProviderMock.Setup( provider => provider.GenerateRefreshToken( user.Id ) )
            .Returns( new RefreshToken( user.Id, "refreshToken", DateTime.UtcNow.AddDays( 7 ) ) );

        _userRepositoryMock.Setup( repo => repo.ContainsAsync( It.IsAny<Expression<Func<User, bool>>>() ) )
            .ReturnsAsync( true );

        // Act
        ResultT<TokenInfoDto> result = await _handler.Handle( command );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.Equal( "jwtToken", result.Value.AccessToken );
        Assert.Equal( "refreshToken", result.Value.RefreshToken );
        _unitOfWorkMock.Verify( uow => uow.Commit(), Times.Once );
    }

    [Fact]
    public async Task Handle_EmptyLogin_Fail()
    {
        // Arrange
        LoginUserCommand command = new LoginUserCommand { Login = "", Password = "ValidPass123" };

        // Act
        ResultT<TokenInfoDto> result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Логин обязателен.", result.ErrorMessages );
    }

    [Fact]
    public async Task Handle_EmptyPassword_Fail()
    {
        // Arrange
        LoginUserCommand command = new LoginUserCommand { Login = "user123", Password = "" };

        // Act
        ResultT<TokenInfoDto> result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Пароль не может быть пустым.", result.ErrorMessages );
    }

    [Fact]
    public async Task Handle_NonExistentUser_Fail()
    {
        // Arrange
        LoginUserCommand command = new LoginUserCommand { Login = "nonexistent", Password = "ValidPass123" };

        _userRepositoryMock.Setup( repo => repo.ContainsAsync( It.IsAny<Expression<Func<User, bool>>>() ) )
            .ReturnsAsync( false );

        // Act
        ResultT<TokenInfoDto> result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Пользователь не найден.", result.ErrorMessages );
    }

    [Fact]
    public async Task Handle_IncorrectPassword_Fail()
    {
        // Arrange
        LoginUserCommand command = new LoginUserCommand { Login = "user123", Password = "WrongPassword" };
        User user = new User( "User Name", "user123", "hashedPassword" );

        _userRepositoryMock.Setup( repo => repo.GetByLogin( command.Login ) )
            .ReturnsAsync( user );

        _passwordHasherMock.Setup( hasher => hasher.VerifyPassword( command.Password, user.Password ) )
            .Returns( false );

        _userRepositoryMock.Setup( repo => repo.ContainsAsync( It.IsAny<Expression<Func<User, bool>>>() ) )
            .ReturnsAsync( true );

        // Act
        ResultT<TokenInfoDto> result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Неверный логин или пароль.", result.ErrorMessages );
    }
}

