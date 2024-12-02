using System.Linq.Expressions;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.UseCases.Likes.Commands.Create;
using AutoMapper;
using Domain.Entities;
using Moq;

namespace Tests.Likes.Commands;

public class CreateLikeCommandHandlerTests
{
    private readonly Mock<ILikeRepository> _favoriteRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRecipeRepository> _recipeRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly CreateLikeCommandValidator _validator;
    private readonly CreateLikeCommandHandler _handler;

    public CreateLikeCommandHandlerTests()
    {
        _favoriteRepositoryMock = new Mock<ILikeRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _recipeRepositoryMock = new Mock<IRecipeRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();

        _validator = new CreateLikeCommandValidator(
            _recipeRepositoryMock.Object,
            _favoriteRepositoryMock.Object,
            _userRepositoryMock.Object
        );

        _handler = new CreateLikeCommandHandler(
            _favoriteRepositoryMock.Object,
            _validator,
            _unitOfWorkMock.Object,
            _mapperMock.Object );
    }

    [Fact]
    public async Task Handle_ValidCommand_SaveLike()
    {
        // Arrange
        CreateLikeCommand command = new CreateLikeCommand() { RecipeId = 1, UserId = 1 };
        Like favorite = new Like( command.UserId, command.RecipeId );

        _recipeRepositoryMock
            .Setup( r => r.ContainsAsync( It.IsAny<Expression<Func<Recipe, bool>>>() ) )
            .ReturnsAsync( true );
        _userRepositoryMock
            .Setup( u => u.ContainsAsync( It.IsAny<Expression<Func<User, bool>>>() ) )
            .ReturnsAsync( true );
        _favoriteRepositoryMock
            .Setup( f => f.IsRecipeLikedByUser( command.UserId, command.RecipeId ) )
            .ReturnsAsync( false );

        _mapperMock.Setup( m => m.Map<Like>( command ) )
            .Returns( favorite );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.False( result.ErrorMessages.Any() );
        _favoriteRepositoryMock.Verify( r => r.Create( favorite ), Times.Once );
        _unitOfWorkMock.Verify( u => u.Commit(), Times.Once );
    }

    [Theory]
    [InlineData( 0, 1, "Идентификатор рецепта обязателен." )]
    [InlineData( 1, 0, "Идентификатор пользователя обязателен." )]
    public async Task Handle_InvalidRecipeIdOrUserId_Fail( int recipeId, int userId, string expectedError )
    {
        // Arrange
        CreateLikeCommand command = new CreateLikeCommand { RecipeId = recipeId, UserId = userId };

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( expectedError, result.ErrorMessages );
        _favoriteRepositoryMock.Verify( r => r.Create( It.IsAny<Like>() ), Times.Never );
        _unitOfWorkMock.Verify( u => u.Commit(), Times.Never );
    }

    [Fact]
    public async Task Handle_RecipeDoesNotExist_Fail()
    {
        // Arrange
        CreateLikeCommand command = new CreateLikeCommand() { RecipeId = 1, UserId = 1 };
        _recipeRepositoryMock
            .Setup( r => r.ContainsAsync( It.IsAny<Expression<Func<Recipe, bool>>>() ) )
            .ReturnsAsync( false );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.True( result.ErrorMessages.Any() );
        Assert.Contains( "Рецепт с Id 1 не найден.", result.ErrorMessages );
        _favoriteRepositoryMock.Verify( r => r.Create( It.IsAny<Like>() ), Times.Never );
        _unitOfWorkMock.Verify( u => u.Commit(), Times.Never );
    }

    [Fact]
    public async Task Handle_RecipeAlreadyInLikes_Fail()
    {
        // Arrange
        CreateLikeCommand command = new CreateLikeCommand() { RecipeId = 1, UserId = 1 };
        _recipeRepositoryMock
            .Setup( r => r.ContainsAsync( It.IsAny<Expression<Func<Recipe, bool>>>() ) )
            .ReturnsAsync( true );
        _userRepositoryMock
            .Setup( u => u.ContainsAsync( It.IsAny<Expression<Func<User, bool>>>() ) )
            .ReturnsAsync( true );
        _favoriteRepositoryMock
            .Setup( f => f.IsRecipeLikedByUser( command.UserId, command.RecipeId ) )
            .ReturnsAsync( true );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.True( result.ErrorMessages.Any() );
        Assert.Contains( "Этот рецепт уже добавлен в понравившееся.", result.ErrorMessages );
        _favoriteRepositoryMock.Verify( r => r.Create( It.IsAny<Like>() ), Times.Never );
        _unitOfWorkMock.Verify( u => u.Commit(), Times.Never );
    }
}