using Application.Interfaces.Repositories;
using Application.UseCases.Tags.Dtos;
using Application.UseCases.Tags.Queries.GetByName;
using AutoMapper;
using Domain.Entities;
using FluentValidation.Results;
using FluentValidation;
using Moq;
using Application.Common.Result;

namespace Tests.Tags.Queries;

public class GetTagByNameQueryHandlerTests
{
    private readonly Mock<ITagRepository> _tagRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetTagByNameQueryHandler _handler;
    private readonly GetTagByNameQueryValidator _validator;

    public GetTagByNameQueryHandlerTests()
    {
        _tagRepositoryMock = new Mock<ITagRepository>();
        _mapperMock = new Mock<IMapper>();
        _validator = new GetTagByNameQueryValidator( _tagRepositoryMock.Object );
        _handler = new GetTagByNameQueryHandler( _tagRepositoryMock.Object, _validator, _mapperMock.Object );
    }

    [Fact]
    public async Task Handle_ReturnsMappedTag_WhenValidationSucceedsAndTagExists()
    {
        // Arrange
        string tagName = "TestTag";
        GetTagByNameQuery query = new GetTagByNameQuery { Tag = tagName };
        Tag tag = new Tag( tagName ) { Id = 1 };
        GetTagDto tagDto = new GetTagDto { Id = 1, Name = tagName };

        _tagRepositoryMock.Setup( repo => repo.GetByName( query.Tag ) )
            .ReturnsAsync( tag );
        _mapperMock.Setup( mapper => mapper.Map<GetTagDto>( tag ) )
            .Returns( tagDto );

        // Act
        ResultT<GetTagDto> result = await _handler.Handle( query );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.Equal( tagDto, result.Value );
        Assert.Equal( $"Тег {tagName} получен.", result.SuccessMessage );
        _mapperMock.Verify( mapper => mapper.Map<GetTagDto>( tag ), Times.Once );
    }

    [Fact]
    public async Task Handle_ReturnsValidationError_WhenTagIsEmpty()
    {
        // Arrange
        GetTagByNameQuery query = new GetTagByNameQuery { Tag = "" };

        // Act
        ResultT<GetTagDto> result = await _handler.Handle( query );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Тег обязателен.", result.ErrorMessages );
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenTagDoesNotExist()
    {
        // Arrange
        GetTagByNameQuery query = new GetTagByNameQuery { Tag = "NonExistentTag" };

        _tagRepositoryMock.Setup( repo => repo.GetByName( query.Tag ) )
            .ReturnsAsync( ( Tag )null );

        // Act
        ResultT<GetTagDto> result = await _handler.Handle( query );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Тег не найден.", result.ErrorMessages );
    }
}
