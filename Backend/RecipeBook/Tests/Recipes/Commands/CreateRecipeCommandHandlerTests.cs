using System.Linq.Expressions;
using Application.Common.CQRS.Command;
using Application.Common.FileHelper;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.UseCases.Ingredients.Commands.Create;
using Application.UseCases.Recipes.Commands.Create;
using Application.UseCases.Recipes.Dtos;
using Application.UseCases.Steps.Commands.Create;
using Application.UseCases.Tags.Commands.Create;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Tests.Recipes.Commands;

public class CreateRecipeCommandHandlerTests
{
    private readonly Mock<IRecipeRepository> _recipeRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICommandHandler<CreateTagCommand, Result>> _createTagHandlerMock;
    private readonly Mock<ICommandHandler<CreateStepCommand, Result>> _createStepHandlerMock;
    private readonly Mock<ICommandHandler<CreateIngredientCommand, Result>> _createIngredientHandlerMock;
    private readonly Mock<IFileHelper> _fileHelperMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateRecipeCommandHandler _handler;
    private readonly CreateRecipeCommandValidator _validator;

    public CreateRecipeCommandHandlerTests()
    {
        _recipeRepositoryMock = new Mock<IRecipeRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _createTagHandlerMock = new Mock<ICommandHandler<CreateTagCommand, Result>>();
        _createStepHandlerMock = new Mock<ICommandHandler<CreateStepCommand, Result>>();
        _createIngredientHandlerMock = new Mock<ICommandHandler<CreateIngredientCommand, Result>>();
        _fileHelperMock = new Mock<IFileHelper>();
        _configurationMock = new Mock<IConfiguration>();
        _mapperMock = new Mock<IMapper>();
        _validator = new CreateRecipeCommandValidator( _userRepositoryMock.Object );

        _handler = new CreateRecipeCommandHandler(
            _recipeRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _createTagHandlerMock.Object,
            _createStepHandlerMock.Object,
            _createIngredientHandlerMock.Object,
            _validator,
            _fileHelperMock.Object,
            _configurationMock.Object,
            _mapperMock.Object
        );

        _mapperMock.Setup( m => m.Map<CreateTagCommand>( It.IsAny<RecipeTagDto>() ) )
            .Returns( ( RecipeTagDto dto ) => new CreateTagCommand { Name = dto.Name } );

        _mapperMock.Setup( m => m.Map<CreateStepCommand>( It.IsAny<RecipeStepDto>() ) )
            .Returns( ( RecipeStepDto dto ) => new CreateStepCommand { Description = dto.Description } );

        _mapperMock.Setup( m => m.Map<CreateIngredientCommand>( It.IsAny<RecipeIngredientDto>() ) )
            .Returns( ( RecipeIngredientDto dto ) => new CreateIngredientCommand { Title = dto.Title, Description = dto.Description } );

        _configurationMock.Setup( c => c.GetSection( "RecipeImages" ).Value )
            .Returns( "TestPath" );
    }

    [Fact]
    public async Task Handle_AddTag_Fail()
    {
        // Arrange
        CreateRecipeCommand command = CreateValidCommand();

        _userRepositoryMock
            .Setup( r => r.ContainsAsync( It.IsAny<Expression<Func<User, bool>>>() ) )
            .ReturnsAsync( true );

        _createTagHandlerMock.Setup( h => h.Handle( It.IsAny<CreateTagCommand>() ) )
            .ReturnsAsync( Result.Fail( "Tag error" ) );

        // Act
        var result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Tag error", result.ErrorMessages );
        _recipeRepositoryMock.Verify( r => r.Create( It.IsAny<Recipe>() ), Times.Never );
        _unitOfWorkMock.Verify( u => u.Commit(), Times.Never );
    }

    [Fact]
    public async Task Handle_AddStep_Fail()
    {
        // Arrange
        CreateRecipeCommand command = CreateValidCommand();
        _userRepositoryMock
            .Setup( r => r.ContainsAsync( It.IsAny<Expression<Func<User, bool>>>() ) )
            .ReturnsAsync( true );

        _createTagHandlerMock.Setup( h => h.Handle( It.IsAny<CreateTagCommand>() ) )
            .ReturnsAsync( Result.Success() );

        _createStepHandlerMock.Setup( h => h.Handle( It.IsAny<CreateStepCommand>() ) )
            .ReturnsAsync( Result.Fail( "Step error" ) );

        // Act
        var result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Step error", result.ErrorMessages );
        _recipeRepositoryMock.Verify( r => r.Create( It.IsAny<Recipe>() ), Times.Never );
        _unitOfWorkMock.Verify( u => u.Commit(), Times.Never );
    }

    [Fact]
    public async Task Handle_AddIngredient_Fail()
    {
        // Arrange
        CreateRecipeCommand command = CreateValidCommand();

        _userRepositoryMock
            .Setup( r => r.ContainsAsync( It.IsAny<Expression<Func<User, bool>>>() ) )
            .ReturnsAsync( true );

        _createTagHandlerMock.Setup( h => h.Handle( It.IsAny<CreateTagCommand>() ) )
            .ReturnsAsync( Result.Success() );

        _createStepHandlerMock.Setup( h => h.Handle( It.IsAny<CreateStepCommand>() ) )
            .ReturnsAsync( Result.Success() );

        _createIngredientHandlerMock.Setup( h => h.Handle( It.IsAny<CreateIngredientCommand>() ) )
            .ReturnsAsync( Result.Fail( "Ingredient error" ) );

        // Act
        var result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Ingredient error", result.ErrorMessages );
        _recipeRepositoryMock.Verify( r => r.Create( It.IsAny<Recipe>() ), Times.Never );
        _unitOfWorkMock.Verify( u => u.Commit(), Times.Never );
    }

    [Fact]
    public async Task Handle_ValidCommand_SaveRecipe()
    {
        // Arrange
        CreateRecipeCommand command = CreateValidCommand();

        _userRepositoryMock
            .Setup( r => r.ContainsAsync( It.IsAny<Expression<Func<User, bool>>>() ) )
            .ReturnsAsync( true );

        _createTagHandlerMock.Setup( h => h.Handle( It.IsAny<CreateTagCommand>() ) )
            .ReturnsAsync( Result.Success() );

        _createStepHandlerMock.Setup( h => h.Handle( It.IsAny<CreateStepCommand>() ) )
            .ReturnsAsync( Result.Success() );

        _createIngredientHandlerMock.Setup( h => h.Handle( It.IsAny<CreateIngredientCommand>() ) )
            .ReturnsAsync( Result.Success() );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.True( result.IsSuccess );
        _recipeRepositoryMock.Verify( r => r.Create( It.IsAny<Recipe>() ), Times.Once );
        _unitOfWorkMock.Verify( uow => uow.Commit(), Times.Once );
    }

    [Fact]
    public async Task Validate_InvalidAuthorId_Fail()
    {
        // Arrange
        CreateRecipeCommand command = new CreateRecipeCommand
        {
            AuthorId = 0,
            Name = "Valid Recipe",
            Description = "Valid Description",
            CookTime = 30,
            PortionCount = 4,
            ImageName = "image.jpg",
            Steps = new List<RecipeStepDto> { new RecipeStepDto { Description = "Step1" } },
            Ingredients = new List<RecipeIngredientDto> { new RecipeIngredientDto { Title = "Ingredient1" } },
            Tags = new List<RecipeTagDto> { new RecipeTagDto { Name = "Tag1" } }
        };

        SetupCommonMocks();

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Идентификатор должен быть положительным числом.", result.ErrorMessages );
        _recipeRepositoryMock.Verify( r => r.Create( It.IsAny<Recipe>() ), Times.Never );
        _unitOfWorkMock.Verify( uow => uow.Commit(), Times.Never );
    }

    [Fact]
    public async Task Validate_UserNotFound_Fail()
    {
        // Arrange
        CreateRecipeCommand command = CreateValidCommand();
        _userRepositoryMock.Setup( repo => repo.ContainsAsync( It.IsAny<Expression<Func<User, bool>>>() ) )
            .ReturnsAsync( false );

        _createTagHandlerMock.Setup( h => h.Handle( It.IsAny<CreateTagCommand>() ) )
            .ReturnsAsync( Result.Success() );

        _createStepHandlerMock.Setup( h => h.Handle( It.IsAny<CreateStepCommand>() ) )
            .ReturnsAsync( Result.Success() );

        _createIngredientHandlerMock.Setup( h => h.Handle( It.IsAny<CreateIngredientCommand>() ) )
            .ReturnsAsync( Result.Success() );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Пользователь не найден.", result.ErrorMessages );
        _recipeRepositoryMock.Verify( r => r.Create( It.IsAny<Recipe>() ), Times.Never );
        _unitOfWorkMock.Verify( uow => uow.Commit(), Times.Never );
    }

    [Fact]
    public async Task Handle_EmptyName_Fail()
    {
        // Arrange
        CreateRecipeCommand command = new CreateRecipeCommand
        {
            AuthorId = 1,
            Name = "",
            Description = "Valid Description",
            CookTime = 30,
            PortionCount = 4,
            ImageName = "image.jpg",
            Steps = new List<RecipeStepDto> { new RecipeStepDto { Description = "Step1" } },
            Ingredients = new List<RecipeIngredientDto> { new RecipeIngredientDto { Title = "Ingredient1" } },
            Tags = new List<RecipeTagDto> { new RecipeTagDto { Name = "Tag1" } }
        };

        SetupCommonMocks();

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Название рецепта обязательно.", result.ErrorMessages );
        _recipeRepositoryMock.Verify( r => r.Create( It.IsAny<Recipe>() ), Times.Never );
        _unitOfWorkMock.Verify( uow => uow.Commit(), Times.Never );
    }

    [Fact]
    public async Task Handle_NameExceedsMaxLength_Fail()
    {
        // Arrange
        CreateRecipeCommand command = new CreateRecipeCommand
        {
            AuthorId = 1,
            Name = new string( 'a', 101 ),
            Description = "Valid Description",
            CookTime = 30,
            PortionCount = 4,
            ImageName = "image.jpg",
            Steps = new List<RecipeStepDto> { new RecipeStepDto { Description = "Step1" } },
            Ingredients = new List<RecipeIngredientDto> { new RecipeIngredientDto { Title = "Ingredient1" } },
            Tags = new List<RecipeTagDto> { new RecipeTagDto { Name = "Tag1" } }
        };

        SetupCommonMocks();

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Название рецепта не может превышать 100 символов.", result.ErrorMessages );
        _recipeRepositoryMock.Verify( r => r.Create( It.IsAny<Recipe>() ), Times.Never );
        _unitOfWorkMock.Verify( uow => uow.Commit(), Times.Never );
    }

    [Fact]
    public async Task Validate_DescriptionIsEmpty_Fail()
    {
        // Arrange
        CreateRecipeCommand command = new CreateRecipeCommand
        {
            AuthorId = 1,
            Name = "Valid Recipe",
            Description = "",
            CookTime = 30,
            PortionCount = 4,
            ImageName = "image.jpg",
            Steps = new List<RecipeStepDto> { new RecipeStepDto { Description = "Step1" } },
            Ingredients = new List<RecipeIngredientDto> { new RecipeIngredientDto { Title = "Ingredient1" } },
            Tags = new List<RecipeTagDto> { new RecipeTagDto { Name = "Tag1" } }
        };

        SetupCommonMocks();

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Описание рецепта обязательно.", result.ErrorMessages );
        _recipeRepositoryMock.Verify( r => r.Create( It.IsAny<Recipe>() ), Times.Never );
        _unitOfWorkMock.Verify( uow => uow.Commit(), Times.Never );
    }

    [Fact]
    public async Task Validate_DescriptionExceedsMaxLength_Fail()
    {
        // Arrange
        CreateRecipeCommand command = new CreateRecipeCommand
        {
            AuthorId = 1,
            Name = "Valid Recipe",
            Description = new string( 'a', 151 ),
            CookTime = 30,
            PortionCount = 4,
            ImageName = "image.jpg",
            Steps = new List<RecipeStepDto> { new RecipeStepDto { Description = "Step1" } },
            Ingredients = new List<RecipeIngredientDto> { new RecipeIngredientDto { Title = "Ingredient1" } },
            Tags = new List<RecipeTagDto> { new RecipeTagDto { Name = "Tag1" } }
        };

        SetupCommonMocks();

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Описание не может превышать 150 символов.", result.ErrorMessages );
        _recipeRepositoryMock.Verify( r => r.Create( It.IsAny<Recipe>() ), Times.Never );
        _unitOfWorkMock.Verify( uow => uow.Commit(), Times.Never );
    }

    [Fact]
    public async Task Validate_CookTimeIsZeroOrNegative_Fail()
    {
        // Arrange
        CreateRecipeCommand command = new CreateRecipeCommand
        {
            AuthorId = 1,
            Name = "Valid Recipe",
            Description = "Valid Description",
            CookTime = 0,
            PortionCount = 4,
            ImageName = "image.jpg",
            Steps = new List<RecipeStepDto> { new RecipeStepDto { Description = "Step1" } },
            Ingredients = new List<RecipeIngredientDto> { new RecipeIngredientDto { Title = "Ingredient1" } },
            Tags = new List<RecipeTagDto> { new RecipeTagDto { Name = "Tag1" } }
        };

        SetupCommonMocks();

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Время готовки должно быть больше 0.", result.ErrorMessages );
        _recipeRepositoryMock.Verify( r => r.Create( It.IsAny<Recipe>() ), Times.Never );
        _unitOfWorkMock.Verify( uow => uow.Commit(), Times.Never );
    }

    [Fact]
    public async Task Validate_PortionCountIsZeroOrNegative_Fail()
    {
        // Arrange
        CreateRecipeCommand command = new CreateRecipeCommand
        {
            AuthorId = 1,
            Name = "Valid Recipe",
            Description = "Valid Description",
            CookTime = 30,
            PortionCount = 0,
            ImageName = "image.jpg",
            Steps = new List<RecipeStepDto> { new RecipeStepDto { Description = "Step1" } },
            Ingredients = new List<RecipeIngredientDto> { new RecipeIngredientDto { Title = "Ingredient1" } },
            Tags = new List<RecipeTagDto> { new RecipeTagDto { Name = "Tag1" } }
        };

        SetupCommonMocks();

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Количество порций должно быть больше 0.", result.ErrorMessages );
        _recipeRepositoryMock.Verify( r => r.Create( It.IsAny<Recipe>() ), Times.Never );
        _unitOfWorkMock.Verify( uow => uow.Commit(), Times.Never );
    }

    [Fact]
    public async Task Validate_ImageNameIsEmpty_Fail()
    {
        // Arrange
        CreateRecipeCommand command = new CreateRecipeCommand
        {
            AuthorId = 1,
            Name = "Valid Recipe",
            Description = "Valid Description",
            CookTime = 30,
            PortionCount = 4,
            ImageName = "",
            Steps = new List<RecipeStepDto> { new RecipeStepDto { Description = "Step1" } },
            Ingredients = new List<RecipeIngredientDto> { new RecipeIngredientDto { Title = "Ingredient1" } },
            Tags = new List<RecipeTagDto> { new RecipeTagDto { Name = "Tag1" } }
        };

        SetupCommonMocks();

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Картинка обязательна.", result.ErrorMessages );
        _recipeRepositoryMock.Verify( r => r.Create( It.IsAny<Recipe>() ), Times.Never );
        _unitOfWorkMock.Verify( uow => uow.Commit(), Times.Never );
    }

    [Fact]
    public async Task Validate_CommandIsValid_SaveRecipe()
    {
        // Arrange
        CreateRecipeCommand command = CreateValidCommand();

        SetupCommonMocks();

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.True( result.IsSuccess );
        _recipeRepositoryMock.Verify( r => r.Create( It.IsAny<Recipe>() ), Times.Once );
        _unitOfWorkMock.Verify( uow => uow.Commit(), Times.Once );
    }

    private CreateRecipeCommand CreateValidCommand()
    {
        return new CreateRecipeCommand
        {
            AuthorId = 1,
            Name = "Test Recipe",
            Description = "Test Description",
            CookTime = 30,
            PortionCount = 4,
            ImageName = "image.jpg",
            ImageFile = Mock.Of<IFormFile>(),
            Tags = new List<RecipeTagDto> { new RecipeTagDto { Name = "Tag1" } },
            Steps = new List<RecipeStepDto> { new RecipeStepDto { Description = "Step1" } },
            Ingredients = new List<RecipeIngredientDto> { new RecipeIngredientDto { Title = "Ingredient1", Description = "Ingredient1Description" } }
        };
    }

    private void SetupCommonMocks()
    {
        _userRepositoryMock.Setup( repo => repo.ContainsAsync( It.IsAny<Expression<Func<User, bool>>>() ) )
            .ReturnsAsync( true );

        _createTagHandlerMock.Setup( h => h.Handle( It.IsAny<CreateTagCommand>() ) )
            .ReturnsAsync( Result.Success() );

        _createStepHandlerMock.Setup( h => h.Handle( It.IsAny<CreateStepCommand>() ) )
            .ReturnsAsync( Result.Success() );

        _createIngredientHandlerMock.Setup( h => h.Handle( It.IsAny<CreateIngredientCommand>() ) )
            .ReturnsAsync( Result.Success() );
    }
}
