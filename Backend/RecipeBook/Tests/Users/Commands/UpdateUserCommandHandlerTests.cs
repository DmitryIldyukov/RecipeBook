using Application.Common.PasswordHasher;
using Application.Interfaces.Repositories;
using Application.Interfaces;
using Application.UseCases.Users.Commands.Update;
using Domain.Entities;
using Moq;
using System.Linq.Expressions;
using Application.Common.Result;

namespace Tests.Users.Commands;

public class UpdateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IPasswordHasher> _hasherMock;
    private readonly UpdateUserCommandValidator _validator;
    private readonly UpdateUserCommandHandler _handler;

    public UpdateUserCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _hasherMock = new Mock<IPasswordHasher>();
        _validator = new UpdateUserCommandValidator( _userRepositoryMock.Object );
        _handler = new UpdateUserCommandHandler(
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _validator,
            _hasherMock.Object
        );
    }

    [Fact]
    public async Task Handle_ReturnsSuccess_WhenDataIsValid()
    {
        // Arrange
        UpdateUserCommand command = new UpdateUserCommand
        {
            UserId = 1,
            Name = "Updated Name",
            Login = "UpdatedLogin",
            Password = "NewPassword123",
            Information = "Updated information"
        };
        User user = new User( "Original Name", "OriginalLogin", "OriginalPassword" ) { Id = 1 };

        _userRepositoryMock.Setup( repo => repo.GetById( command.UserId ) )
            .ReturnsAsync( user );

        _hasherMock.Setup( hasher => hasher.HashPassword( command.Password ) )
            .Returns( "HashedPassword" );

        _userRepositoryMock.Setup( repo => repo.ContainsAsync( It.Is<Expression<Func<User, bool>>>( expr =>
            expr.Compile().Invoke( user ) ) ) )
            .ReturnsAsync( true );

        _userRepositoryMock.Setup( repo => repo.ContainsAsync( It.Is<Expression<Func<User, bool>>>( expr =>
            expr.Compile().Invoke( new User( "Other Name", "UpdatedLogin", "OtherPassword" ) { Id = 2 } ) ) ) )
            .ReturnsAsync( false );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.Equal( "Updated Name", user.Name );
        Assert.Equal( "UpdatedLogin", user.Login );
        Assert.Equal( "Updated information", user.Information );
        Assert.Equal( "HashedPassword", user.Password );
        _unitOfWorkMock.Verify( uow => uow.Commit(), Times.Once );
    }

    [Fact]
    public async Task Handle_ReturnsValidationError_WhenUserIdIsInvalid()
    {
        // Arrange
        UpdateUserCommand command = new UpdateUserCommand { UserId = 0, Name = "Test Name", Login = "TestLogin" };

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Идентификатор автора обязателен", result.ErrorMessages );
    }

    [Fact]
    public async Task Handle_ReturnsValidationError_WhenNameIsEmpty()
    {
        // Arrange
        UpdateUserCommand command = new UpdateUserCommand { UserId = 1, Name = "", Login = "ValidLogin" };

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Имя автора не может быть пустым", result.ErrorMessages );
    }

    [Fact]
    public async Task Handle_ReturnsValidationError_WhenLoginIsNotUnique()
    {
        // Arrange
        UpdateUserCommand command = new UpdateUserCommand { UserId = 1, Name = "Valid Name", Login = "ExistingLogin" };

        _userRepositoryMock.Setup( repo => repo.ContainsAsync( It.IsAny<Expression<Func<User, bool>>>() ) )
            .ReturnsAsync( true );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Пользователь с таким логином уже существует", result.ErrorMessages );
    }

    [Fact]
    public async Task Handle_DoesNotUpdatePassword_WhenPasswordIsEmpty()
    {
        // Arrange
        UpdateUserCommand command = new UpdateUserCommand
        {
            UserId = 1,
            Name = "Updated Name",
            Login = "Updated Login",
            Password = "",
            Information = "Updated information"
        };
        User user = new User( "Original Name", "Original Login", "Original Password" ) { Id = 1 };

        _userRepositoryMock.Setup( repo => repo.GetById( command.UserId ) )
            .ReturnsAsync( user );

        _userRepositoryMock.Setup( repo => repo.ContainsAsync( It.Is<Expression<Func<User, bool>>>( expr =>
            expr.Compile().Invoke( user ) ) ) )
            .ReturnsAsync( true );

        _userRepositoryMock.Setup( repo => repo.ContainsAsync( It.Is<Expression<Func<User, bool>>>( expr =>
            expr.Compile().Invoke( new User( "Other Name", "UpdatedLogin", "OtherPassword" ) { Id = 2 } ) ) ) )
            .ReturnsAsync( false );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.Equal( "Updated Name", user.Name );
        Assert.Equal( "Updated Login", user.Login );
        Assert.Equal( "Updated information", user.Information );
        Assert.Equal( "Original Password", user.Password );
        _unitOfWorkMock.Verify( uow => uow.Commit(), Times.Once );
    }
}

