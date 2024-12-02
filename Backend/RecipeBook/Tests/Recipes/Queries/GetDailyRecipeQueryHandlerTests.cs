using Application.Common.Result;
using Application.Interfaces.Repositories;
using Application.UseCases.Recipes.Dtos;
using Application.UseCases.Recipes.Queries.GetDailyRecipe;
using AutoMapper;
using Domain.Entities;
using Moq;

namespace Tests.Recipes.Queries;

public class GetDailyRecipeQueryHandlerTests
{
    private readonly Mock<IRecipeRepository> _recipeRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetDailyRecipeQueryHandler _handler;

    public GetDailyRecipeQueryHandlerTests()
    {
        _recipeRepositoryMock = new Mock<IRecipeRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetDailyRecipeQueryHandler( _recipeRepositoryMock.Object, _mapperMock.Object );
    }

    [Fact]
    public async Task Handle_DailyRecipeExists_ReturnsDailyRecipe()
    {
        // Arrange
        Recipe recipe = new Recipe( 1, "Рецепт", "Описание", 30, 4, "image.jpg" ) { Id = 1 };
        DailyRecipeDto dailyRecipeDto = new DailyRecipeDto
        {
            RecipeId = 1,
            AuthorId = 1,
            Name = "Рецепт",
            Description = "Описание",
            CookTime = 30
        };

        _recipeRepositoryMock
            .Setup( repo => repo.GetDailyRecipe() )
            .ReturnsAsync( recipe );

        _mapperMock
            .Setup( mapper => mapper.Map<DailyRecipeDto>( recipe ) )
            .Returns( dailyRecipeDto );

        GetDailyRecipeQuery query = new GetDailyRecipeQuery();

        // Act
        ResultT<DailyRecipeDto> result = await _handler.Handle( query );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.Equal( dailyRecipeDto, result.Value );
    }

    [Fact]
    public async Task Handle_DailyRecipeDoesNotExist_Fail()
    {
        // Arrange
        _recipeRepositoryMock
            .Setup( repo => repo.GetDailyRecipe() )
            .ReturnsAsync( ( Recipe )null );

        GetDailyRecipeQuery query = new GetDailyRecipeQuery();

        // Act
        ResultT<DailyRecipeDto> result = await _handler.Handle( query );

        // Assert
        Assert.False( result.IsSuccess );
    }
}
