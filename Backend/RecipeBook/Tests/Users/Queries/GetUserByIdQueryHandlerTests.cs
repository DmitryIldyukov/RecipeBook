using Application.Interfaces.Repositories;
using Application.Interfaces;
using Application.UseCases.Users.Dtos;
using Application.UseCases.Users.Queries.GetById;
using AutoMapper;
using Domain.Entities;
using Moq;
using System.Linq.Expressions;
using Application.Common.Result;

namespace Tests.Users.Queries;

public class GetUserByIdQueryHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetUserByIdQueryValidator _validator;
    private readonly GetUserByIdQueryHandler _handler;

    public GetUserByIdQueryHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _validator = new GetUserByIdQueryValidator( _userRepositoryMock.Object );
        _handler = new GetUserByIdQueryHandler(
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _validator,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_ReturnsSuccess_WhenUserExists()
    {
        // Arrange
        GetUserByIdQuery query = new GetUserByIdQuery { Id = 1 };
        User user = new User( "Test User", "testuser", "password" ) { Id = 1 };
        GetUserQueryDto userDto = new GetUserQueryDto { Id = 1, Name = "Test User", Login = "testuser" };

        _userRepositoryMock.Setup( repo => repo.GetById( query.Id ) )
            .ReturnsAsync( user );

        _mapperMock.Setup( mapper => mapper.Map<GetUserQueryDto>( user ) )
            .Returns( userDto );

        _userRepositoryMock.Setup( repo => repo.ContainsAsync( It.IsAny<Expression<Func<User, bool>>>() ) ).ReturnsAsync( true );

        // Act
        ResultT<GetUserQueryDto> result = await _handler.Handle( query );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.Equal( userDto, result.Value );
        _unitOfWorkMock.Verify( uow => uow.Commit(), Times.Once );
    }

    [Fact]
    public async Task Handle_ReturnsValidationError_WhenIdIsZeroOrNegative()
    {
        // Arrange
        GetUserByIdQuery query = new GetUserByIdQuery { Id = 0 };

        // Act
        ResultT<GetUserQueryDto> result = await _handler.Handle( query );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Идентификатор должен быть положительным числом.", result.ErrorMessages );
    }

    [Fact]
    public async Task Handle_ReturnsValidationError_WhenUserDoesNotExist()
    {
        // Arrange
        GetUserByIdQuery query = new GetUserByIdQuery { Id = 1 };

        _userRepositoryMock.Setup( repo => repo.ContainsAsync( It.IsAny<Expression<Func<User, bool>>>() ) )
            .ReturnsAsync( false );

        // Act
        ResultT<GetUserQueryDto> result = await _handler.Handle( query );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Пользователь не найден.", result.ErrorMessages );
    }

    [Fact]
    public async Task Handle_CommitsTransaction_WhenUserIsFound()
    {
        // Arrange
        GetUserByIdQuery query = new GetUserByIdQuery { Id = 1 };
        User user = new User( "Test User", "testuser", "password" ) { Id = 1 };
        GetUserQueryDto userDto = new GetUserQueryDto { Id = 1, Name = "Test User", Login = "testuser" };

        _userRepositoryMock.Setup( repo => repo.GetById( query.Id ) )
            .ReturnsAsync( user );

        _mapperMock.Setup( mapper => mapper.Map<GetUserQueryDto>( user ) )
            .Returns( userDto );

        _userRepositoryMock.Setup( repo => repo.ContainsAsync( It.IsAny<Expression<Func<User, bool>>>() ) ).ReturnsAsync( true );

        // Act
        ResultT<GetUserQueryDto> result = await _handler.Handle( query );

        // Assert
        Assert.True( result.IsSuccess );
        _unitOfWorkMock.Verify( uow => uow.Commit(), Times.Once );
    }
}

