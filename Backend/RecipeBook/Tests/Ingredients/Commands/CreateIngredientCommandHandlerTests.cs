using Application.Common.Result;
using Application.Interfaces.Repositories;
using Application.UseCases.Ingredients.Commands.Create;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Tests.Ingredients.Commands;

public class CreateIngredientCommandHandlerTests
{
    private readonly Mock<IIngredientRepository> _ingredientRepositoryMock;
    private readonly Mock<IValidator<CreateIngredientCommand>> _validatorMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateIngredientCommandHandler _handler;

    public CreateIngredientCommandHandlerTests()
    {
        _ingredientRepositoryMock = new Mock<IIngredientRepository>();
        _validatorMock = new Mock<IValidator<CreateIngredientCommand>>();
        _mapperMock = new Mock<IMapper>();

        _handler = new CreateIngredientCommandHandler(
            _ingredientRepositoryMock.Object,
            _validatorMock.Object,
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

        _validatorMock
            .Setup( v => v.ValidateAsync( command, It.IsAny<CancellationToken>() ) )
            .ReturnsAsync( new ValidationResult() );

        _mapperMock
            .Setup( m => m.Map<Ingredient>( command ) )
            .Returns( ingredient );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.Equal( $"Ингредиент {ingredient.Title} успешно добавлен.", result.SuccessMessage );
        Assert.Contains( ingredient, recipe.Ingredients );
        _ingredientRepositoryMock.Verify( r => r.Create( ingredient ), Times.Once );
    }

    [Fact]
    public async Task Handle_InvalidCommand_Fail()
    {
        // Arrange
        CreateIngredientCommand command = new CreateIngredientCommand();

        List<ValidationFailure> validationErrors = new List<ValidationFailure>
        {
            new ValidationFailure("Title", "Заголовок ингредиента обязателен."),
            new ValidationFailure("Description", "Описание ингредиента обязательно.")
        };

        _validatorMock
            .Setup( v => v.ValidateAsync( command, It.IsAny<CancellationToken>() ) )
            .ReturnsAsync( new ValidationResult( validationErrors ) );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Заголовок ингредиента обязателен.", result.ErrorMessages );
        Assert.Contains( "Описание ингредиента обязательно.", result.ErrorMessages );
        _ingredientRepositoryMock.Verify( r => r.Create( It.IsAny<Ingredient>() ), Times.Never );
    }
}
