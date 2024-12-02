using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Common.CQRS.Command;
using Application.Common.FileHelper;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.UseCases.Recipes.Commands.Delete;
using Application.UseCases.Tags.Commands.Delete;
using Domain.Entities;
using Moq;
using Xunit;

namespace Tests.Recipes.Commands;

public class DeleteRecipeCommandHandlerTests
{
    private readonly Mock<IRecipeRepository> _recipeRepositoryMock;
    private readonly Mock<ITagRepository> _tagRepositoryMock;
    private readonly Mock<ICommandHandler<DeleteTagsCommand, Result>> _deleteTagsHandlerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IFileHelper> _fileHelperMock;
    private readonly DeleteRecipeCommandValidator _validator;
    private readonly DeleteRecipeCommandHandler _handler;

    public DeleteRecipeCommandHandlerTests()
    {
        _recipeRepositoryMock = new Mock<IRecipeRepository>();
        _tagRepositoryMock = new Mock<ITagRepository>();
        _validator = new DeleteRecipeCommandValidator( _recipeRepositoryMock.Object );
        _deleteTagsHandlerMock = new Mock<ICommandHandler<DeleteTagsCommand, Result>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _fileHelperMock = new Mock<IFileHelper>();

        _handler = new DeleteRecipeCommandHandler(
            _recipeRepositoryMock.Object,
            _validator,
            _deleteTagsHandlerMock.Object,
            _unitOfWorkMock.Object,
            _fileHelperMock.Object
        );
    }

    [Fact]
    public async Task Handle_RecipeIdIsInvalid_Fail()
    {
        // Arrange
        int recipeId = -1;

        DeleteRecipeCommand command = new DeleteRecipeCommand { RecipeId = recipeId };
        Recipe recipe = new Recipe( 1, "Торт", "Шоколадный торт", 60, 8, "cake.jpg" ) { Id = recipeId, Tags = new List<Tag> { new Tag( "Tag1" ) { Id = 1 } } };

        _recipeRepositoryMock
            .Setup( r => r.ContainsAsync( It.IsAny<Expression<Func<Recipe, bool>>>() ) )
            .ReturnsAsync( true );
        _recipeRepositoryMock
            .Setup( repo => repo.GetById( recipeId ) )
            .ReturnsAsync( recipe );
        _tagRepositoryMock
            .Setup( repo => repo.IsUsedInMultipleRecipes( It.IsAny<int>() ) )
            .ReturnsAsync( false );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Идентификатор должен быть положительным числом.", result.ErrorMessages );
        _recipeRepositoryMock.Verify( r => r.Delete( It.IsAny<Recipe>() ), Times.Never );
        _fileHelperMock.Verify( fh => fh.Delete( It.IsAny<string>() ), Times.Never );
        _unitOfWorkMock.Verify( uow => uow.Commit(), Times.Never );
    }

    [Fact]
    public async Task Handle_RecipeNotFound_Fail()
    {
        // Arrange
        DeleteRecipeCommand command = new DeleteRecipeCommand { RecipeId = 1 };

        _recipeRepositoryMock
            .Setup( r => r.ContainsAsync( It.IsAny<Expression<Func<Recipe, bool>>>() ) )
            .ReturnsAsync( false );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( $"Рецепт с Id {command.RecipeId} не найден.", result.ErrorMessages );
        _recipeRepositoryMock.Verify( r => r.Delete( It.IsAny<Recipe>() ), Times.Never );
        _fileHelperMock.Verify( fh => fh.Delete( It.IsAny<string>() ), Times.Never );
        _unitOfWorkMock.Verify( uow => uow.Commit(), Times.Never );
    }

    [Fact]
    public async Task Handle_RecipeExists_DeleteRcipe()
    {
        // Arrange
        DeleteRecipeCommand command = new DeleteRecipeCommand { RecipeId = 1 };
        Recipe recipe = new Recipe( 1, "Торт", "Шоколадный торт", 60, 8, "cake.jpg" ) { Id = 1, Tags = new List<Tag> { new Tag( "Tag1" ) { Id = 1 } } };

        _recipeRepositoryMock
            .Setup( r => r.ContainsAsync( It.IsAny<Expression<Func<Recipe, bool>>>() ) )
            .ReturnsAsync( true );

        _recipeRepositoryMock.Setup( repo => repo.GetById( command.RecipeId ) )
                             .ReturnsAsync( recipe );

        _deleteTagsHandlerMock.Setup( dth => dth.Handle( It.IsAny<DeleteTagsCommand>() ) )
                              .ReturnsAsync( Result.Success() );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.True( result.IsSuccess );
        _deleteTagsHandlerMock.Verify( dth => dth.Handle( It.IsAny<DeleteTagsCommand>() ), Times.Once );
        _recipeRepositoryMock.Verify( repo => repo.Delete( It.IsAny<Recipe>() ), Times.Once );
        _unitOfWorkMock.Verify( uow => uow.Commit(), Times.Once );
        _fileHelperMock.Verify( fh => fh.Delete( It.IsAny<string>() ), Times.Once );
    }

    [Fact]
    public async Task Handle_DeleteTagsFails_Fail()
    {
        // Arrange
        int recipeId = 1;

        DeleteRecipeCommand command = new DeleteRecipeCommand { RecipeId = recipeId };
        Recipe recipe = new Recipe( 1, "Торт", "Шоколадный торт", 60, 8, "cake.jpg" ) { Id = recipeId, Tags = new List<Tag> { new Tag( "Tag1" ) { Id = 1 } } };

        _recipeRepositoryMock
            .Setup( r => r.ContainsAsync( It.IsAny<Expression<Func<Recipe, bool>>>() ) )
            .ReturnsAsync( true );
        _recipeRepositoryMock
            .Setup( repo => repo.GetById( recipeId ) )
            .ReturnsAsync( recipe );
        _tagRepositoryMock
            .Setup( repo => repo.IsUsedInMultipleRecipes( It.IsAny<int>() ) )
            .ReturnsAsync( false );

        _deleteTagsHandlerMock.Setup( dth => dth.Handle( It.IsAny<DeleteTagsCommand>() ) )
                              .ReturnsAsync( Result.Fail( "Ошибка при удалении тегов" ) );

        // Act
        Result result = await _handler.Handle( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Ошибка при удалении тегов", result.ErrorMessages );
        _recipeRepositoryMock.Verify( repo => repo.Delete( It.IsAny<Recipe>() ), Times.Never );
        _unitOfWorkMock.Verify( uow => uow.Commit(), Times.Never );
        _fileHelperMock.Verify( fh => fh.Delete( It.IsAny<string>() ), Times.Never );
    }
}
