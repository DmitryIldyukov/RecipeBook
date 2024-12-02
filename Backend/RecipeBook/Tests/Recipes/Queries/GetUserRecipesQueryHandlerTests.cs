using Application.Common.Result;
using Application.Interfaces.Repositories;
using Application.UseCases.Recipes.Dtos;
using Application.UseCases.Recipes.Queries.GetUserRecipes;
using AutoMapper;
using Domain.Entities;
using Moq;

namespace Tests.Recipes.Queries;

public class GetUserRecipesQueryHandlerTests
{
    private readonly Mock<IRecipeRepository> _recipeRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetUserRecipesQueryValidator _validator;
    private readonly GetUserRecipesQueryHandler _handler;

    public GetUserRecipesQueryHandlerTests()
    {
        _recipeRepositoryMock = new Mock<IRecipeRepository>();
        _mapperMock = new Mock<IMapper>();
        _validator = new GetUserRecipesQueryValidator();
        _handler = new GetUserRecipesQueryHandler(
            _recipeRepositoryMock.Object,
            _validator,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_UserHasRecipes_ReturnsUserRecipes()
    {
        // Arrange
        GetUserRecipesQuery query = new GetUserRecipesQuery { UserId = 1 };
        List<Recipe> recipes = new List<Recipe>
        {
            new Recipe( 1, "Рецепт", "Описание", 30, 4, "image.jpg" ) { Id = 1, Likes = new List<Like> { new Like(1, 1) }, Favorites = new List<Favorite>() },
            new Recipe( 1, "Рецепт", "Описание", 30, 4, "image.jpg" ) { Id = 2, Likes = new List<Like>(), Favorites = new List<Favorite> { new Favorite( 1, 1 ) } }
        };

        List<GetRecipeQueryDto> recipeDtos = recipes.Select( r => new GetRecipeQueryDto { RecipeId = r.Id, IsLiked = true, IsFavorite = false } ).ToList();

        _recipeRepositoryMock.Setup( repo => repo.GetUserRecipes( query.UserId ) )
            .ReturnsAsync( recipes );

        _mapperMock.Setup( m => m.Map<GetRecipeQueryDto>( It.IsAny<Recipe>() ) )
            .Returns( ( Recipe recipe ) => new GetRecipeQueryDto { RecipeId = recipe.Id } );

        // Act
        ResultT<IReadOnlyList<GetRecipeQueryDto>> result = await _handler.Handle( query );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.Equal( recipes.Count, result.Value.Count );
    }

    [Fact]
    public async Task Handle_UserIdIsInvalid_Fail()
    {
        // Arrange
        GetUserRecipesQuery query = new GetUserRecipesQuery { UserId = 0 };

        // Act
        ResultT<IReadOnlyList<GetRecipeQueryDto>> result = await _handler.Handle( query );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Идентификатор пользователя обязателен.", result.ErrorMessages );
    }

    [Fact]
    public async Task Handle_NoRecipesFound_ReturnsEmptyListOfRecipes()
    {
        // Arrange
        GetUserRecipesQuery query = new GetUserRecipesQuery { UserId = 1 };

        _recipeRepositoryMock.Setup( repo => repo.GetUserRecipes( query.UserId ) )
            .ReturnsAsync( new List<Recipe>() );

        // Act
        ResultT<IReadOnlyList<GetRecipeQueryDto>> result = await _handler.Handle( query );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.Empty( result.Value );
    }
}

