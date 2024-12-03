using System.Linq.Expressions;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using Application.UseCases.Tags.Commands.Delete;
using Domain.Entities;
using Moq;

namespace Tests.Tags.Commands;

public class DeleteTagsCommandHandlerTests
{
    private readonly Mock<IRecipeRepository> _recipeRepositoryMock;
    private readonly Mock<ITagRepository> _tagRepositoryMock;
    private readonly DeleteTagsCommandHandler _handler;
    private readonly DeleteTagsCommandValidator _validator;

    public DeleteTagsCommandHandlerTests()
    {
        _recipeRepositoryMock = new Mock<IRecipeRepository>();
        _tagRepositoryMock = new Mock<ITagRepository>();
        _validator = new DeleteTagsCommandValidator( _recipeRepositoryMock.Object );
        _handler = new DeleteTagsCommandHandler( _validator, _tagRepositoryMock.Object, _recipeRepositoryMock.Object );
    }

    [Fact]
    public async Task Handle_ValidCommand_DeletesTagsFromRecipeAndRepository()
    {
        // Arrange
        int recipeId = 1;
        List<Tag> tags = new List<Tag>
        {
            new Tag("Tag1") { Id = 1 },
            new Tag("Tag2") { Id = 2 }
        };
        Recipe recipe = new Recipe( 1, "Рецепт", "Описание", 30, 4, "image.jpg" ) { Id = recipeId, Tags = tags.ToList() };

        DeleteTagsCommand command = new DeleteTagsCommand { RecipeId = recipeId, Tags = tags };

        _recipeRepositoryMock.Setup( repo => repo.ContainsAsync( It.IsAny<Expression<Func<Recipe, bool>>>() ) ).ReturnsAsync( true );
        _recipeRepositoryMock.Setup( repo => repo.GetById( recipeId ) ).ReturnsAsync( recipe );
        _tagRepositoryMock.Setup( repo => repo.IsUsedInMultipleRecipes( It.IsAny<int>() ) ).ReturnsAsync( false );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.Empty( recipe.Tags );
        _tagRepositoryMock.Verify( repo => repo.Delete( It.IsAny<Tag>() ), Times.Exactly( tags.Count ) );
    }

    [Fact]
    public async Task Handle_TagUsedInMultipleRecipes_RemovesTagOnlyFromRecipe()
    {
        // Arrange
        int recipeId = 1;
        Tag tag = new Tag( "Tag1" ) { Id = 1 };
        Recipe recipe = new Recipe( 1, "Рецепт", "Описание", 30, 4, "image.jpg" ) { Id = recipeId, Tags = new List<Tag> { tag } };

        DeleteTagsCommand command = new DeleteTagsCommand { RecipeId = recipeId, Tags = new List<Tag> { tag } };

        _recipeRepositoryMock.Setup( repo => repo.ContainsAsync( It.IsAny<Expression<Func<Recipe, bool>>>() ) ).ReturnsAsync( true );
        _recipeRepositoryMock.Setup( repo => repo.GetById( recipeId ) ).ReturnsAsync( recipe );
        _tagRepositoryMock.Setup( repo => repo.IsUsedInMultipleRecipes( tag.Id ) ).ReturnsAsync( true );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.Empty( recipe.Tags );
        _tagRepositoryMock.Verify( repo => repo.Delete( It.IsAny<Tag>() ), Times.Never );
    }

    [Fact]
    public async Task Handle_InvalidRecipeId_Fail()
    {
        // Arrange
        DeleteTagsCommand command = new DeleteTagsCommand { RecipeId = 0, Tags = null };

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Идентификатор рецепта обязателен.", result.ErrorMessages );
        Assert.Contains( "Идентификатор должен быть положительным числом.", result.ErrorMessages );
    }

    [Fact]
    public async Task Handle_NonExistingRecipe_Fail()
    {
        // Arrange
        int recipeId = 1;
        DeleteTagsCommand command = new DeleteTagsCommand { RecipeId = recipeId, Tags = new List<Tag> { new Tag( "Tag1" ) { Id = 1 } } };

        _recipeRepositoryMock.Setup( repo => repo.ContainsAsync( It.IsAny<Expression<Func<Recipe, bool>>>() ) ).ReturnsAsync( false );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( $"Рецепт с Id {recipeId} не найден.", result.ErrorMessages );
    }
}

