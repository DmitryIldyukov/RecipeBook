using System.Linq.Expressions;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.UseCases.Favorites.Commands.Create;
using AutoMapper;
using Domain.Entities;
using Moq;

namespace Tests.Favorites.Commands;

public class CreateFavoriteCommandHandlerTests
{
    private readonly Mock<IFavoriteRepository> _favoriteRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRecipeRepository> _recipeRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly CreateFavoriteCommandValidator _validator;
    private readonly CreateFavoriteCommandHandler _handler;

    public CreateFavoriteCommandHandlerTests()
    {
        _favoriteRepositoryMock = new Mock<IFavoriteRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _recipeRepositoryMock = new Mock<IRecipeRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();

        _validator = new CreateFavoriteCommandValidator(
            _recipeRepositoryMock.Object,
            _favoriteRepositoryMock.Object,
            _userRepositoryMock.Object
        );

        _handler = new CreateFavoriteCommandHandler(
            _favoriteRepositoryMock.Object,
            _validator,
            _unitOfWorkMock.Object,
            _mapperMock.Object );
    }

    [Fact]
    public async Task Handle_ValidCommand_SaveFavorite()
    {
        // Arrange
        CreateFavoriteCommand command = new CreateFavoriteCommand() { RecipeId = 1, UserId = 1 };
        Favorite favorite = new Favorite( command.UserId, command.RecipeId );

        _recipeRepositoryMock
            .Setup( r => r.ContainsAsync( It.IsAny<Expression<Func<Recipe, bool>>>() ) )
            .ReturnsAsync( true );
        _userRepositoryMock
            .Setup( u => u.ContainsAsync( It.IsAny<Expression<Func<User, bool>>>() ) )
            .ReturnsAsync( true );
        _favoriteRepositoryMock
            .Setup( f => f.IsUserFavoriteRecipe( command.UserId, command.RecipeId ) )
            .ReturnsAsync( false );

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

    [Theory]
    [InlineData( 0, 1, "Идентификатор рецепта обязателен." )]
    [InlineData( 1, 0, "Идентификатор пользователя обязателен." )]
    public async Task Handle_InvalidRecipeIdOrUserId_Fail( int recipeId, int userId, string expectedError )
    {
        // Arrange
        CreateFavoriteCommand command = new CreateFavoriteCommand { RecipeId = recipeId, UserId = userId };

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( expectedError, result.ErrorMessages );
        _favoriteRepositoryMock.Verify( r => r.Create( It.IsAny<Favorite>() ), Times.Never );
        _unitOfWorkMock.Verify( u => u.Commit(), Times.Never );
    }

    [Fact]
    public async Task Handle_RecipeDoesNotExist_Fail()
    {
        // Arrange
        CreateFavoriteCommand command = new CreateFavoriteCommand() { RecipeId = 1, UserId = 1 };
        _recipeRepositoryMock
            .Setup( r => r.ContainsAsync( It.IsAny<Expression<Func<Recipe, bool>>>() ) )
            .ReturnsAsync( false );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.True( result.ErrorMessages.Any() );
        Assert.Contains( "Рецепт с Id 1 не найден.", result.ErrorMessages );
        _favoriteRepositoryMock.Verify( r => r.Create( It.IsAny<Favorite>() ), Times.Never );
        _unitOfWorkMock.Verify( u => u.Commit(), Times.Never );
    }

    [Fact]
    public async Task Handle_RecipeAlreadyInFavorites_Fail()
    {
        // Arrange
        CreateFavoriteCommand command = new CreateFavoriteCommand() { RecipeId = 1, UserId = 1 };
        _recipeRepositoryMock
            .Setup( r => r.ContainsAsync( It.IsAny<Expression<Func<Recipe, bool>>>() ) )
            .ReturnsAsync( true );
        _userRepositoryMock
            .Setup( u => u.ContainsAsync( It.IsAny<Expression<Func<User, bool>>>() ) )
            .ReturnsAsync( true );
        _favoriteRepositoryMock
            .Setup( f => f.IsUserFavoriteRecipe( command.UserId, command.RecipeId ) )
            .ReturnsAsync( true );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.True( result.ErrorMessages.Any() );
        Assert.Contains( "Этот рецепт уже добавлен в избранное.", result.ErrorMessages );
        _favoriteRepositoryMock.Verify( r => r.Create( It.IsAny<Favorite>() ), Times.Never );
        _unitOfWorkMock.Verify( u => u.Commit(), Times.Never );
    }
}