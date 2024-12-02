using Application.Common.Result;
using Application.Interfaces.Repositories;
using Application.UseCases.Tags.Commands.Create;
using Domain.Entities;
using Moq;

namespace Tests.Tags.Commands;

public class CreateTagCommandHandlerTests
{
    private readonly Mock<ITagRepository> _tagRepositoryMock;
    private readonly CreateTagCommandHandler _handler;
    private readonly CreateTagCommandValidator _validator;

    public CreateTagCommandHandlerTests()
    {
        _tagRepositoryMock = new Mock<ITagRepository>();
        _validator = new CreateTagCommandValidator();
        _handler = new CreateTagCommandHandler( _tagRepositoryMock.Object, _validator );
    }

    [Fact]
    public async Task Handle_ValidCommand_TagAlreadyExists_AddsExistingTagToRecipe()
    {
        // Arrange
        string tagName = "существующий тег";
        Tag tag = new Tag( tagName );

        Recipe recipe = new Recipe( 1, "Рецепт", "Описание", 30, 4, "image.jpg" );

        CreateTagCommand command = new CreateTagCommand { Name = tagName, Recipe = recipe };

        _tagRepositoryMock
            .Setup( repo => repo.GetByName( tagName.ToLower() ) )
            .ReturnsAsync( tag );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.Contains( tag, recipe.Tags );
        _tagRepositoryMock.Verify( repo => repo.Create( It.IsAny<Tag>() ), Times.Never );
    }

    [Fact]
    public async Task Handle_ValidCommand_TagDoesNotExist_CreateAndAddNewTagToRecipe()
    {
        // Arrange
        string tagName = "новый тег";
        Recipe recipe = new Recipe( 1, "Рецепт", "Описание", 30, 4, "image.jpg" );

        CreateTagCommand command = new CreateTagCommand { Name = tagName, Recipe = recipe };

        _tagRepositoryMock
            .Setup( repo => repo.GetByName( tagName.ToLower() ) )
            .ReturnsAsync( ( Tag )null );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.True( result.IsSuccess );
        _tagRepositoryMock.Verify( repo => repo.Create( It.Is<Tag>( t => t.Name == tagName.ToLower() ) ), Times.Once );
        Assert.Contains( recipe.Tags, t => t.Name == tagName.ToLower() );
    }

    [Fact]
    public async Task Handle_InvalidNameOrRecipe_Fail()
    {
        // Arrange
        var command = new CreateTagCommand { Name = "", Recipe = null };

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Тег не может быть пустым.", result.ErrorMessages );
        Assert.Contains( "Рецепт обязателен.", result.ErrorMessages );
        _tagRepositoryMock.Verify( repo => repo.Create( It.IsAny<Tag>() ), Times.Never );
    }

    [Fact]
    public async Task Handle_LongTagName_Fail()
    {
        // Arrange
        string longTagName = new string( 'a', 31 );
        Recipe recipe = new Recipe( 1, "Рецепт", "Описание", 30, 4, "image.jpg" );

        CreateTagCommand command = new CreateTagCommand { Name = longTagName, Recipe = recipe };

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Максимальная длина тега 30 символов.", result.ErrorMessages );
        _tagRepositoryMock.Verify( repo => repo.Create( It.IsAny<Tag>() ), Times.Never );
    }
}
