using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.UseCases.Favorites.Commands.Create;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Tests.Favorites.Commands;

public class CreateFavoriteCommandHandlerTests
{
    private readonly Mock<IFavoriteRepository> _favoriteRepositoryMock;
    private readonly Mock<IValidator<CreateFavoriteCommand>> _validatorMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateFavoriteCommandHandler _handler;

    public CreateFavoriteCommandHandlerTests()
    {
        _favoriteRepositoryMock = new Mock<IFavoriteRepository>();
        _validatorMock = new Mock<IValidator<CreateFavoriteCommand>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();

        _handler = new CreateFavoriteCommandHandler(
            _favoriteRepositoryMock.Object,
            _validatorMock.Object,
            _unitOfWorkMock.Object,
            _mapperMock.Object );
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateFavoriteAndCommit()
    {
        // Arrange
        CreateFavoriteCommand command = new CreateFavoriteCommand() { RecipeId = 1, UserId = 1 };
        Favorite favorite = new Favorite( command.UserId, command.RecipeId );
        _validatorMock.Setup( v => v.ValidateAsync( command, default ) )
            .ReturnsAsync( new ValidationResult() );
        _mapperMock.Setup( m => m.Map<Favorite>( command ) )
            .Returns( favorite );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.False( result.ErrorMessages.Any() );
        _favoriteRepositoryMock.Verify( r => r.Create( favorite ), Times.Once );
        _unitOfWorkMock.Verify( u => u.Commit(), Times.Once );
    }

    [Fact]
    public async Task Handle_InvalidCommand_ShouldReturnFail()
    {
        // Arrange
        CreateFavoriteCommand command = new CreateFavoriteCommand();
        _validatorMock.Setup( v => v.ValidateAsync( command, default ) )
            .ReturnsAsync( new ValidationResult( new[]
            {
                new ValidationFailure( "RecipeId", "Идентификатор рецепта обязателен." ),
                new ValidationFailure( "UserId", "Идентификатор пользователя обязаелен." ),
            } ) );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.True( result.ErrorMessages.Any() );
        Assert.False( result.IsSuccess );
        _favoriteRepositoryMock.Verify( r => r.Create( It.IsAny<Favorite>() ), Times.Never );
        _unitOfWorkMock.Verify( u => u.Commit(), Times.Never );
    }
}