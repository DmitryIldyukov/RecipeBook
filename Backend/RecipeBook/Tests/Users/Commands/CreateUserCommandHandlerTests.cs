using System.Linq.Expressions;
using Application.Common.PasswordHasher;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.UseCases.Users.Commands.Create;
using Domain.Entities;
using Moq;

namespace Tests.Users.Commands;

public class CreateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly CreateUserCommandValidator _validator;
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _validator = new CreateUserCommandValidator( _userRepositoryMock.Object );
        _handler = new CreateUserCommandHandler( _userRepositoryMock.Object, _unitOfWorkMock.Object, _validator, _passwordHasherMock.Object );
    }

    [Fact]
    public async Task Handle_ValidCommand_SaveUser()
    {
        // Arrange
        CreateUserCommand command = new CreateUserCommand { Name = "John Doe", Login = "johndoe", Password = "StrongPass123" };
        _passwordHasherMock.Setup( h => h.HashPassword( command.Password ) ).Returns( "hashedPassword" );

        _userRepositoryMock.Setup( repo => repo.ContainsAsync( It.IsAny<Expression<Func<User, bool>>>() ) )
            .ReturnsAsync( false );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.True( result.IsSuccess );
        _userRepositoryMock.Verify( repo => repo.Create( It.Is<User>( u => u.Login == command.Login ) ), Times.Once );
        _unitOfWorkMock.Verify( uow => uow.Commit(), Times.Once );
    }

    [Fact]
    public async Task Handle_NonUniqueLogin_Fail()
    {
        // Arrange
        CreateUserCommand command = new CreateUserCommand { Name = "Jane Doe", Login = "janedoe", Password = "StrongPass123" };

        _userRepositoryMock.Setup( repo => repo.ContainsAsync( It.IsAny<Expression<Func<User, bool>>>() ) )
            .ReturnsAsync( true );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Пользователь с таким логином уже существует.", result.ErrorMessages );
        _userRepositoryMock.Verify( repo => repo.Create( It.IsAny<User>() ), Times.Never );
        _unitOfWorkMock.Verify( uow => uow.Commit(), Times.Never );
    }

    [Fact]
    public async Task Handle_ShortPassword_Fail()
    {
        // Arrange
        CreateUserCommand command = new CreateUserCommand { Name = "John Doe", Login = "johndoe", Password = "short" };

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Пароль должен состоять минимум из 8 символов.", result.ErrorMessages );
        _userRepositoryMock.Verify( repo => repo.Create( It.IsAny<User>() ), Times.Never );
        _unitOfWorkMock.Verify( uow => uow.Commit(), Times.Never );
    }

    [Fact]
    public async Task Handle_EmptyName_Fail()
    {
        // Arrange
        CreateUserCommand command = new CreateUserCommand { Name = "", Login = "newuser", Password = "StrongPass123" };

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Имя автора не может быть пустым", result.ErrorMessages );
        _userRepositoryMock.Verify( repo => repo.Create( It.IsAny<User>() ), Times.Never );
        _unitOfWorkMock.Verify( uow => uow.Commit(), Times.Never );
    }

    [Fact]
    public async Task Handle_EmptyPassword_Fail()
    {
        // Arrange
        CreateUserCommand command = new CreateUserCommand { Name = "John Doe", Login = "newuser", Password = "" };

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Пароль не может быть пустым.", result.ErrorMessages );
        _userRepositoryMock.Verify( repo => repo.Create( It.IsAny<User>() ), Times.Never );
        _unitOfWorkMock.Verify( uow => uow.Commit(), Times.Never );
    }
}

