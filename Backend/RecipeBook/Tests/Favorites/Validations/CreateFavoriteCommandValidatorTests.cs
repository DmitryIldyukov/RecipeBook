using Application.Interfaces.Repositories;
using Application.UseCases.Favorites.Commands.Delete;
using FluentValidation.Results;
using Moq;

namespace Tests.Favorites.Validation;

public class DeleteFavoriteCommandValidatorTests
{
    private readonly Mock<IFavoriteRepository> _favoriteRepositoryMock;
    private readonly DeleteFavoriteCommandValidator _validator;

    public DeleteFavoriteCommandValidatorTests()
    {
        _favoriteRepositoryMock = new Mock<IFavoriteRepository>();
        _validator = new DeleteFavoriteCommandValidator( _favoriteRepositoryMock.Object );
    }

    [Fact]
    public async Task Validate_ValidCommand_PassValidation()
    {
        // Arrange
        DeleteFavoriteCommand command = new DeleteFavoriteCommand { UserId = 1, RecipeId = 1 };

        _favoriteRepositoryMock
            .Setup( r => r.IsUserFavoriteRecipe( command.UserId, command.RecipeId ) )
            .ReturnsAsync( true );

        // Act
        ValidationResult result = await _validator.ValidateAsync( command );

        // Assert
        Assert.True( result.IsValid );
        Assert.Empty( result.Errors );
    }

    [Fact]
    public async Task Validate_MissingUserId_FailValidation()
    {
        // Arrange
        DeleteFavoriteCommand command = new DeleteFavoriteCommand { RecipeId = 1 };

        // Act
        ValidationResult result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsValid );
        Assert.Contains( result.Errors, e => e.PropertyName == "UserId" && e.ErrorMessage == "Идентификатор пользователя обязателен." );
    }

    [Fact]
    public async Task Validate_MissingRecipeId_FailValidation()
    {
        // Arrange
        DeleteFavoriteCommand command = new DeleteFavoriteCommand { UserId = 1 };

        // Act
        ValidationResult result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsValid );
        Assert.Contains( result.Errors, e => e.PropertyName == "RecipeId" && e.ErrorMessage == "Идентификатор рецепта обязателен." );
    }

    [Fact]
    public async Task Validate_FavoriteDoesNotExist_FailValidation()
    {
        // Arrange
        DeleteFavoriteCommand command = new DeleteFavoriteCommand { UserId = 1, RecipeId = 1 };

        _favoriteRepositoryMock
            .Setup( r => r.IsUserFavoriteRecipe( command.UserId, command.RecipeId ) )
            .ReturnsAsync( false );

        // Act
        ValidationResult result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsValid );
        Assert.Contains( result.Errors, e => e.ErrorMessage == "Избранный рецепт не найден." );
    }
}
