using System.Linq.Expressions;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using Application.UseCases.Ingredients.Commands.UpdateIngredient;
using Domain.Entities;
using Moq;

namespace Tests.Ingredients.Commands;

public class UpdateIngredientCommandHandlerTests
{
    private readonly Mock<IIngredientRepository> _ingredientRepositoryMock;
    private readonly UpdateIngredientCommandValidator _validator;
    private readonly UpdateIngredientCommandHandler _handler;

    public UpdateIngredientCommandHandlerTests()
    {
        _ingredientRepositoryMock = new Mock<IIngredientRepository>();
        _validator = new UpdateIngredientCommandValidator( _ingredientRepositoryMock.Object );
        _handler = new UpdateIngredientCommandHandler(
            _ingredientRepositoryMock.Object,
            _validator
        );
    }

    [Fact]
    public async Task Handle_ValidCommand_UpdateIngredient()
    {
        // Arrange
        Ingredient ingredient = new Ingredient( 1, "Соль", "Щепотка соли" );
        _ingredientRepositoryMock.Setup( r => r.GetById( It.IsAny<int>() ) ).ReturnsAsync( ingredient );
        _ingredientRepositoryMock
            .Setup( u => u.ContainsAsync( It.IsAny<Expression<Func<Ingredient, bool>>>() ) )
            .ReturnsAsync( true );

        UpdateIngredientCommand command = new UpdateIngredientCommand
        {
            IngredientId = 1,
            Title = "Морская соль",
            Description = "Мелкая морская соль"
        };

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.Equal( "Морская соль", ingredient.Title );
        Assert.Equal( "Мелкая морская соль", ingredient.Description );
        _ingredientRepositoryMock.Verify( r => r.GetById( 1 ), Times.Once );
    }

    [Fact]
    public async Task Handle_MissingTitle_Fail()
    {
        // Arrange
        Ingredient ingredient = new Ingredient( 1, "Соль", "Щепотка соли" );
        _ingredientRepositoryMock.Setup( r => r.GetById( It.IsAny<int>() ) ).ReturnsAsync( ingredient );

        UpdateIngredientCommand command = new UpdateIngredientCommand
        {
            IngredientId = ingredient.Id,
            Title = string.Empty,
            Description = "Мелкая морская соль"
        };

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Заголовок ингредиента обязателен.", result.ErrorMessages );
    }

    [Fact]
    public async Task Handle_TitleTooLong_Fail()
    {
        // Arrange
        Ingredient ingredient = new Ingredient( 1, "Соль", "Щепотка соли" );
        _ingredientRepositoryMock.Setup( r => r.GetById( It.IsAny<int>() ) ).ReturnsAsync( ingredient );

        UpdateIngredientCommand command = new UpdateIngredientCommand
        {
            IngredientId = ingredient.Id,
            Title = new string( 'A', 41 ),
            Description = "Мелкая морская соль"
        };

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Заголовок ингредиента не может превышать 40 символов.", result.ErrorMessages );
    }

    [Fact]
    public async Task Handle_MissingDescription_Fail()
    {
        // Arrange
        Ingredient ingredient = new Ingredient( 1, "Соль", "Щепотка соли" );
        _ingredientRepositoryMock.Setup( r => r.GetById( It.IsAny<int>() ) ).ReturnsAsync( ingredient );

        UpdateIngredientCommand command = new UpdateIngredientCommand
        {
            IngredientId = ingredient.Id,
            Title = "Морская соль",
            Description = string.Empty
        };

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Описание ингредиента обязательно.", result.ErrorMessages );
    }

    [Fact]
    public async Task Handle_InvalidIngredientId_Fail()
    {
        // Arrange
        _ingredientRepositoryMock.Setup( r => r.GetById( It.IsAny<int>() ) ).ReturnsAsync( ( Ingredient )null );

        UpdateIngredientCommand command = new UpdateIngredientCommand
        {
            IngredientId = 999,
            Title = "Морская соль",
            Description = "Мелкая морская соль"
        };

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( $"Ингредиент с Id {command.IngredientId} не найден.", result.ErrorMessages );
    }

    [Fact]
    public async Task Handle_NegativeIngredientId_Fail()
    {
        // Arrange
        UpdateIngredientCommand command = new UpdateIngredientCommand
        {
            IngredientId = -1,
            Title = "Морская соль",
            Description = "Мелкая морская соль"
        };

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Идентификатор должен быть положительным числом.", result.ErrorMessages );
    }
}