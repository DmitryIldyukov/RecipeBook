using System.Linq.Expressions;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using Application.UseCases.Recipes.Dtos;
using Application.UseCases.Recipes.Queries.GetById;
using AutoMapper;
using Domain.Entities;
using Moq;

namespace Tests.Recipes.Queries;

public class GetRecipeByIdQueryHandlerTests
{
    private readonly Mock<IRecipeRepository> _recipeRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetRecipeByIdQueryValidator _validator;
    private readonly GetRecipeByIdQueryHandler _handler;

    public GetRecipeByIdQueryHandlerTests()
    {
        _recipeRepositoryMock = new Mock<IRecipeRepository>();
        _mapperMock = new Mock<IMapper>();
        _validator = new GetRecipeByIdQueryValidator( _recipeRepositoryMock.Object );
        _handler = new GetRecipeByIdQueryHandler(
            _recipeRepositoryMock.Object,
            _validator,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_ReturnsSuccess_WhenRecipeExists()
    {
        // Arrange
        int recipeId = 1;

        GetRecipeByIdQuery query = new GetRecipeByIdQuery { RecipeId = recipeId };
        Recipe recipe = new Recipe( 1, "Рецепт", "Описание", 30, 4, "image.jpg" ) { Id = recipeId };
        GetRecipeQueryDto recipeDto = new GetRecipeQueryDto { RecipeId = recipeId, Name = "Тестовый рецепт" };

        _recipeRepositoryMock.Setup( repo => repo.GetById( recipeId ) )
            .ReturnsAsync( recipe );

        _recipeRepositoryMock
            .Setup( r => r.ContainsAsync( It.IsAny<Expression<Func<Recipe, bool>>>() ) )
            .ReturnsAsync( true );

        _mapperMock.Setup( mapper => mapper.Map<GetRecipeQueryDto>( recipe ) )
            .Returns( recipeDto );

        // Act
        ResultT<GetRecipeQueryDto> result = await _handler.Handle( query );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.Equal( recipeDto, result.Value );
    }

    [Fact]
    public async Task Handle_ReturnsSuccess_WithLikeAndFavoriteStatus_WhenUserIdIsProvided()
    {
        // Arrange
        int recipeId = 1;
        int userId = 1;

        GetRecipeByIdQuery query = new GetRecipeByIdQuery { RecipeId = recipeId, UserId = userId };
        Recipe recipe = new Recipe( 1, "Рецепт", "Описание", 30, 4, "image.jpg" )
        {
            Id = recipeId,
            Likes = new List<Like> { new Like( recipeId, userId ) },
            Favorites = new List<Favorite> { new Favorite( recipeId, userId ) }
        };
        GetRecipeQueryDto recipeDto = new GetRecipeQueryDto { RecipeId = recipeId, IsLiked = true, IsFavorite = true };

        _recipeRepositoryMock.Setup( repo => repo.GetById( query.RecipeId ) )
            .ReturnsAsync( recipe );

        _recipeRepositoryMock
            .Setup( r => r.ContainsAsync( It.IsAny<Expression<Func<Recipe, bool>>>() ) )
            .ReturnsAsync( true );

        _mapperMock.Setup( mapper => mapper.Map<GetRecipeQueryDto>( recipe ) )
            .Returns( recipeDto );

        // Act
        ResultT<GetRecipeQueryDto> result = await _handler.Handle( query );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.True( result.Value.IsLiked );
        Assert.True( result.Value.IsFavorite );
    }

    [Fact]
    public async Task Handle_ReturnsValidationError_WhenRecipeIdIsInvalid()
    {
        // Arrange
        int recipeId = 0;

        GetRecipeByIdQuery query = new GetRecipeByIdQuery { RecipeId = recipeId };

        // Act
        ResultT<GetRecipeQueryDto> result = await _handler.Handle( query );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Идентификатор должен быть положительным числом.", result.ErrorMessages );
    }

    [Fact]
    public async Task Handle_ReturnsValidationError_WhenRecipeDoesNotExist()
    {
        // Arrange
        GetRecipeByIdQuery query = new GetRecipeByIdQuery { RecipeId = 1 };

        _recipeRepositoryMock.Setup( repo => repo.ContainsAsync( It.IsAny<Expression<Func<Recipe, bool>>>() ) )
            .ReturnsAsync( false );

        // Act
        ResultT<GetRecipeQueryDto> result = await _handler.Handle( query );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( $"Рецепт с Id {query.RecipeId} не найден.", result.ErrorMessages );
    }
}

