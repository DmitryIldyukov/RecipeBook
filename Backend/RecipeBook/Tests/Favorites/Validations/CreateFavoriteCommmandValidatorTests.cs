using System.Linq.Expressions;
using Application.Interfaces.Repositories;
using Application.UseCases.Favorites.Commands.Create;
using Domain.Entities;
using FluentValidation.Results;
using Moq;

namespace Tests.Favorites.Validation;

public class CreateFavoriteCommandValidatorTests
{
    private readonly Mock<IRecipeRepository> _recipeRepositoryMock;
    private readonly Mock<IFavoriteRepository> _favoriteRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly CreateFavoriteCommandValidator _validator;

    public CreateFavoriteCommandValidatorTests()
    {
        _recipeRepositoryMock = new Mock<IRecipeRepository>();
        _favoriteRepositoryMock = new Mock<IFavoriteRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();

        _validator = new CreateFavoriteCommandValidator(
            _recipeRepositoryMock.Object,
            _favoriteRepositoryMock.Object,
            _userRepositoryMock.Object
        );
    }

    [Fact]
    public async Task Validate_ValidCommand_PassValidation()
    {
        // Arrange
        CreateFavoriteCommand command = new CreateFavoriteCommand { RecipeId = 1, UserId = 1 };
        _recipeRepositoryMock
            .Setup( r => r.ContainsAsync( It.IsAny<Expression<Func<Recipe, bool>>>() ) )
            .ReturnsAsync( true );
        _userRepositoryMock
            .Setup( u => u.ContainsAsync( It.IsAny<Expression<Func<User, bool>>>() ) )
            .ReturnsAsync( true );
        _favoriteRepositoryMock
            .Setup( f => f.IsUserFavoriteRecipe( command.UserId, command.RecipeId ) )
            .ReturnsAsync( false );

        // Act
        ValidationResult result = await _validator.ValidateAsync( command );

        // Assert
        Assert.True( result.IsValid );
    }

    [Fact]
    public async Task Validate_RecipeIdIsEmpty_FailValidation()
    {
        // Arrange
        CreateFavoriteCommand command = new CreateFavoriteCommand { UserId = 1 };

        // Act
        ValidationResult result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsValid );
        Assert.Contains( result.Errors, e => e.ErrorMessage == "Идентификатор рецепта обязателен." );
    }

    [Fact]
    public async Task Validate_UserIdIsEmpty_FailValidation()
    {
        // Arrange
        CreateFavoriteCommand command = new CreateFavoriteCommand { RecipeId = 1 };

        // Act
        ValidationResult result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsValid );
        Assert.Contains( result.Errors, e => e.ErrorMessage == "Идентификатор пользователя обязаелен." );
    }

    [Fact]
    public async Task Validate_RecipeDoesNotExist_FailValidation()
    {
        // Arrange
        CreateFavoriteCommand command = new CreateFavoriteCommand { RecipeId = 1, UserId = 1 };
        _recipeRepositoryMock
            .Setup( r => r.ContainsAsync( It.IsAny<Expression<Func<Recipe, bool>>>() ) )
            .ReturnsAsync( false );

        // Act
        ValidationResult result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsValid );
        Assert.Contains( result.Errors, e => e.ErrorMessage == "Рецепт с Id 1 не найден." );
    }

    [Fact]
    public async Task Validate_UserDoesNotExist_FailValidation()
    {
        // Arrange
        CreateFavoriteCommand command = new CreateFavoriteCommand { RecipeId = 1, UserId = 1 };
        _userRepositoryMock
            .Setup( u => u.ContainsAsync( It.IsAny<Expression<Func<User, bool>>>() ) )
            .ReturnsAsync( false );

        // Act
        ValidationResult result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsValid );
        Assert.Contains( result.Errors, e => e.ErrorMessage == "Пользователь с Id 1 не найден." );
    }

    [Fact]
    public async Task Validate_RecipeAlreadyInFavorites_FailValidation()
    {
        // Arrange
        CreateFavoriteCommand command = new CreateFavoriteCommand { RecipeId = 1, UserId = 1 };
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
        ValidationResult result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsValid );
        Assert.Contains( result.Errors, e => e.ErrorMessage == "Этот рецепт уже добавлен в избранное." );
    }
}

