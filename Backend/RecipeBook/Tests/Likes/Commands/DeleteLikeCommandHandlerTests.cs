using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.UseCases.Likes.Commands.Delete;
using Domain.Entities;
using FluentValidation.Results;
using Moq;

namespace Tests.Likes.Commands;

public class DeleteLikeCommandHandlerTests
{
    private readonly Mock<ILikeRepository> _likeRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteLikeCommandHandler _handler;
    private readonly DeleteLikeCommandValidator _validator;

    public DeleteLikeCommandHandlerTests()
    {
        _likeRepositoryMock = new Mock<ILikeRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _validator = new DeleteLikeCommandValidator( _likeRepositoryMock.Object );

        _handler = new DeleteLikeCommandHandler(
            _likeRepositoryMock.Object,
            _validator,
            _unitOfWorkMock.Object
        );
    }

    [Fact]
    public async Task Handle_ValidCommand_DeleteLike()
    {
        // Arrange
        DeleteLikeCommand command = new DeleteLikeCommand { UserId = 1, RecipeId = 1 };
        Like like = new Like( command.UserId, command.RecipeId );

        _likeRepositoryMock
            .Setup( r => r.IsRecipeLikedByUser( command.UserId, command.RecipeId ) )
            .ReturnsAsync( true );

        _likeRepositoryMock
            .Setup( r => r.GetByUserIdAndRecipeId( command.UserId, command.RecipeId ) )
            .ReturnsAsync( like );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.False( result.ErrorMessages.Any() );
        _likeRepositoryMock.Verify( r => r.Delete( like ), Times.Once );
        _unitOfWorkMock.Verify( u => u.Commit(), Times.Once );
    }

    [Theory]
    [InlineData( 0, 1, "Идентификатор пользователя обязателен." )]
    [InlineData( 1, 0, "Идентификатор рецепта обязателен." )]
    public async Task Validate_MissingRecipeIdOrUserId_Fail( int userId, int recipeId, string expectedError )
    {
        // Arrange
        DeleteLikeCommand command = new DeleteLikeCommand
        {
            UserId = userId,
            RecipeId = recipeId
        };

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( expectedError, result.ErrorMessages );
        _likeRepositoryMock.Verify( r => r.Delete( It.IsAny<Like>() ), Times.Never );
        _unitOfWorkMock.Verify( u => u.Commit(), Times.Never );
    }

    [Fact]
    public async Task Validate_LikeDoesNotExist_Fail()
    {
        // Arrange
        DeleteLikeCommand command = new DeleteLikeCommand { UserId = 1, RecipeId = 1 };

        _likeRepositoryMock
            .Setup( r => r.IsRecipeLikedByUser( command.UserId, command.RecipeId ) )
            .ReturnsAsync( false );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Понравившийся рецепт не найден.", result.ErrorMessages );
        _likeRepositoryMock.Verify( r => r.Delete( It.IsAny<Like>() ), Times.Never );
        _unitOfWorkMock.Verify( u => u.Commit(), Times.Never );
    }
}
