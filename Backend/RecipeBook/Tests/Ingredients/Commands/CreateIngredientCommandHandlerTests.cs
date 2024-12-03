using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.UseCases.Ingredients.Commands.Create;
using AutoMapper;
using Domain.Entities;
using Moq;

namespace Tests.Ingredients.Commands;

public class CreateIngredientCommandHandlerTests
{
    private readonly Mock<IIngredientRepository> _ingredientRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateIngredientCommandValidator _validator;
    private readonly CreateIngredientCommandHandler _handler;

    public CreateIngredientCommandHandlerTests()
    {
        _ingredientRepositoryMock = new Mock<IIngredientRepository>();
        _mapperMock = new Mock<IMapper>();

        _validator = new CreateIngredientCommandValidator();
        _handler = new CreateIngredientCommandHandler(
            _ingredientRepositoryMock.Object,
            _validator,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_ValidCommand_SaveIngredient()
    {
        // Arrange
        Recipe recipe = new Recipe( 1, "Торт", "Шоколадный торт", 60, 8, "cake.jpg" );
        CreateIngredientCommand command = new CreateIngredientCommand
        {
            Recipe = recipe,
            Title = "Сахар",
            Description = "200 грамм сахара"
        };

        Ingredient ingredient = new Ingredient( recipe.Id, command.Title, command.Description );

        _mapperMock.Setup( m => m.Map<Ingredient>( command ) ).Returns( ingredient );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.False( result.ErrorMessages.Any() );
        Assert.Contains( ingredient, recipe.Ingredients );
        _ingredientRepositoryMock.Verify( r => r.Create( ingredient ), Times.Once );
    }

    [Fact]
    public async Task Handle_MissingTitle_Fail()
    {
        // Arrange
        Recipe recipe = new Recipe( 1, "Торт", "Шоколадный торт", 60, 8, "cake.jpg" );
        CreateIngredientCommand command = new CreateIngredientCommand
        {
            Recipe = recipe,
            Title = string.Empty,
            Description = "200 грамм сахара"
        };

        Ingredient ingredient = new Ingredient( recipe.Id, command.Title, command.Description );

        _mapperMock.Setup( m => m.Map<Ingredient>( command ) ).Returns( ingredient );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Заголовок ингредиента обязателен.", result.ErrorMessages );
        Assert.DoesNotContain( ingredient, recipe.Ingredients );
        _ingredientRepositoryMock.Verify( r => r.Create( It.IsAny<Ingredient>() ), Times.Never );
    }

    [Fact]
    public async Task Handle_MissingDescription_Fail()
    {
        // Arrange
        Recipe recipe = new Recipe( 1, "Торт", "Шоколадный торт", 60, 8, "cake.jpg" );
        CreateIngredientCommand command = new CreateIngredientCommand
        {
            Recipe = recipe,
            Title = "Сахар",
            Description = string.Empty
        };

        Ingredient ingredient = new Ingredient( recipe.Id, command.Title, command.Description );

        _mapperMock.Setup( m => m.Map<Ingredient>( command ) ).Returns( ingredient );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Описание ингредиента обязательно.", result.ErrorMessages );
        Assert.DoesNotContain( ingredient, recipe.Ingredients );
        _ingredientRepositoryMock.Verify( r => r.Create( It.IsAny<Ingredient>() ), Times.Never );
    }

    [Fact]
    public async Task Handle_MissingRecipe_Fail()
    {
        // Arrange
        CreateIngredientCommand command = new CreateIngredientCommand
        {
            Recipe = null,
            Title = "Сахар",
            Description = "200 грамм сахара"
        };

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Рецепт обязателен.", result.ErrorMessages );
        _ingredientRepositoryMock.Verify( r => r.Create( It.IsAny<Ingredient>() ), Times.Never );
    }

    [Fact]
    public async Task Validate_TitleTooLong_Fail()
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
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Заголовок ингредиента не может превышать 40 символов.", result.ErrorMessages );
        _ingredientRepositoryMock.Verify( r => r.Create( It.IsAny<Ingredient>() ), Times.Never );
    }
}
