using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.UseCases.Favorites.Commands.Delete;
using Domain.Entities;
using Moq;

namespace Tests.Favorites.Commands;

public class DeleteLikeCommandHandlerTests
{
    private readonly Mock<IFavoriteRepository> _favoriteRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteFavoriteCommandHandler _handler;
    private readonly DeleteFavoriteCommandValidator _validator;

    public DeleteLikeCommandHandlerTests()
    {
        _favoriteRepositoryMock = new Mock<IFavoriteRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _validator = new DeleteFavoriteCommandValidator( _favoriteRepositoryMock.Object );

        _handler = new DeleteFavoriteCommandHandler(
            _favoriteRepositoryMock.Object,
            _validator,
            _unitOfWorkMock.Object
        );
    }

    [Fact]
    public async Task Handle_ValidCommand_DeleteFavorite()
    {
        // Arrange
        DeleteFavoriteCommand command = new DeleteFavoriteCommand { UserId = 1, RecipeId = 1 };
        Favorite favorite = new Favorite( command.UserId, command.RecipeId );

        _favoriteRepositoryMock
            .Setup( r => r.IsUserFavoriteRecipe( command.UserId, command.RecipeId ) )
            .ReturnsAsync( true );

        _favoriteRepositoryMock
            .Setup( r => r.GetByUserIdAndRecipeId( command.UserId, command.RecipeId ) )
            .ReturnsAsync( favorite );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.False( result.ErrorMessages.Any() );
        _favoriteRepositoryMock.Verify( r => r.Delete( favorite ), Times.Once );
        _unitOfWorkMock.Verify( u => u.Commit(), Times.Once );
    }

    [Theory]
    [InlineData( 0, 1, "Идентификатор пользователя обязателен." )]
    [InlineData( 1, 0, "Идентификатор рецепта обязателен." )]
    public async Task Handle_InvalidUserIdOrRecipeId_Fail( int userId, int recipeId, string expectedError )
    {
        // Arrange
        DeleteFavoriteCommand command = new DeleteFavoriteCommand
        {
            UserId = userId,
            RecipeId = recipeId
        };

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( expectedError, result.ErrorMessages );
        _favoriteRepositoryMock.Verify( r => r.Delete( It.IsAny<Favorite>() ), Times.Never );
        _unitOfWorkMock.Verify( u => u.Commit(), Times.Never );
    }

    [Fact]
    public async Task Handle_FavoriteDoesNotExist_Fail()
    {
        // Arrange
        DeleteFavoriteCommand command = new DeleteFavoriteCommand { UserId = 1, RecipeId = 1 };

        _favoriteRepositoryMock
            .Setup( r => r.IsUserFavoriteRecipe( command.UserId, command.RecipeId ) )
            .ReturnsAsync( false );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Избранный рецепт не найден.", result.ErrorMessages );
        _favoriteRepositoryMock.Verify( r => r.Delete( It.IsAny<Favorite>() ), Times.Never );
        _unitOfWorkMock.Verify( u => u.Commit(), Times.Never );
    }
}
