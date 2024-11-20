using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.UseCases.Favorites.Commands.Delete;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Tests.Favorites.Commands;

public class DeleteFavoriteCommandHandlerTests
{
    private readonly Mock<IFavoriteRepository> _favoriteRepositoryMock;
    private readonly Mock<IValidator<DeleteFavoriteCommand>> _validatorMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteFavoriteCommandHandler _handler;

    public DeleteFavoriteCommandHandlerTests()
    {
        _favoriteRepositoryMock = new Mock<IFavoriteRepository>();
        _validatorMock = new Mock<IValidator<DeleteFavoriteCommand>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new DeleteFavoriteCommandHandler(
            _favoriteRepositoryMock.Object,
            _validatorMock.Object,
            _unitOfWorkMock.Object
        );
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldDeleteFavoriteAndCommit()
    {
        // Arrange
        DeleteFavoriteCommand command = new DeleteFavoriteCommand { UserId = 1, RecipeId = 1 };
        Favorite favorite = new Favorite( command.UserId, command.RecipeId );

        _validatorMock.Setup( v => v.ValidateAsync( command, default ) )
            .ReturnsAsync( new ValidationResult() );
        _favoriteRepositoryMock.Setup( r => r.GetByUserIdAndRecipeId( command.UserId, command.RecipeId ) )
            .ReturnsAsync( favorite );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.True( result.IsSuccess );
        _favoriteRepositoryMock.Verify( r => r.Delete( favorite ), Times.Once );
        _unitOfWorkMock.Verify( u => u.Commit(), Times.Once );
    }

    [Fact]
    public async Task Handle_InvalidCommand_ShouldReturnFail()
    {
        // Arrange
        DeleteFavoriteCommand command = new DeleteFavoriteCommand { UserId = 1, RecipeId = 1 };
        ValidationResult validationErrors = new ValidationResult( new[]
        {
            new ValidationFailure( "UserId", "Идентификатор пользователя обязаелен." ),
            new ValidationFailure( "RecipeId", "Идентификатор рецепта обязателен." )
        } );

        _validatorMock.Setup( v => v.ValidateAsync( command, default ) )
            .ReturnsAsync( validationErrors );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.True( result.ErrorMessages.Any() );
        _favoriteRepositoryMock.Verify( r => r.Delete( It.IsAny<Favorite>() ), Times.Never );
        _unitOfWorkMock.Verify( u => u.Commit(), Times.Never );
    }
}
