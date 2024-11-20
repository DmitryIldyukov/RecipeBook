using Application.UseCases.Ingredients.Commands.Create;
using Domain.Entities;
using FluentValidation.Results;

namespace Tests.Ingredients.Validations;

public class CreateIngredientCommandValidatorTests
{
    private readonly CreateIngredientCommandValidator _validator;

    public CreateIngredientCommandValidatorTests()
    {
        _validator = new CreateIngredientCommandValidator();
    }

    [Fact]
    public async Task Validate_ValidCommand_ShouldPassValidation()
    {
        // Arrange
        Recipe recipe = new Recipe( 1, "Торт", "Шоколадный торт", 60, 8, "cake.jpg" );
        CreateIngredientCommand command = new CreateIngredientCommand
        {
            Recipe = recipe,
            Title = "Сахар",
            Description = "200 грамм сахара"
        };

        // Act
        ValidationResult result = await _validator.ValidateAsync( command );

        // Assert
        Assert.True( result.IsValid );
    }

    [Fact]
    public async Task Validate_MissingRecipe_ShouldFailValidation()
    {
        // Arrange
        CreateIngredientCommand command = new CreateIngredientCommand
        {
            Recipe = null,
            Title = "Сахар",
            Description = "200 грамм сахара"
        };

        // Act
        ValidationResult result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsValid );
        Assert.Contains( result.Errors, e => e.ErrorMessage == "Рецепт обязателен." );
    }

    [Fact]
    public async Task Validate_EmptyTitle_ShouldFailValidation()
    {
        // Arrange
        var recipe = new Recipe( 1, "Торт", "Шоколадный торт", 60, 8, "cake.jpg" );
        var command = new CreateIngredientCommand
        {
            Recipe = recipe,
            Title = string.Empty,
            Description = "200 грамм сахара"
        };

        // Act
        ValidationResult result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsValid );
        Assert.Contains( result.Errors, e => e.ErrorMessage == "Заголовок ингредиента обязателен." );
    }

    [Fact]
    public async Task Validate_TitleTooLong_ShouldFailValidation()
    {
        // Arrange
        Recipe recipe = new Recipe( 1, "Торт", "Шоколадный торт", 60, 8, "cake.jpg" );
        CreateIngredientCommand command = new CreateIngredientCommand
        {
            Recipe = recipe,
            Title = new string( 'A', 41 ),
            Description = "200 грамм сахара"
        };

        // Act
        ValidationResult result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsValid );
        Assert.Contains( result.Errors, e => e.ErrorMessage == "Заголовок игредиента не может превышать 40 символов." );
    }

    [Fact]
    public async Task Validate_EmptyDescription_ShouldFailValidation()
    {
        // Arrange
        Recipe recipe = new Recipe( 1, "Торт", "Шоколадный торт", 60, 8, "cake.jpg" );
        CreateIngredientCommand command = new CreateIngredientCommand
        {
            Recipe = recipe,
            Title = "Сахар",
            Description = string.Empty
        };

        // Act
        ValidationResult result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsValid );
        Assert.Contains( result.Errors, e => e.ErrorMessage == "Описание ингредиента обязательно." );
    }
}
