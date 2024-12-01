using Application.Common.Result;
using Application.Interfaces.Repositories;
using Application.UseCases.Tags.Dtos;
using Application.UseCases.Tags.Queries.GetAll;
using AutoMapper;
using Domain.Entities;
using Moq;

namespace Tests.Tags.Queries;

public class GetAllTagsQueryHandlerTests
{
    private readonly Mock<ITagRepository> _tagRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllTagsQueryHandler _handler;

    public GetAllTagsQueryHandlerTests()
    {
        _tagRepositoryMock = new Mock<ITagRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetAllTagsQueryHandler( _tagRepositoryMock.Object, _mapperMock.Object );
    }

    [Fact]
    public async Task Handle_ReturnsMappedTags_WhenTagsExist()
    {
        // Arrange
        List<Tag> tags = new List<Tag>
        {
            new Tag("Tag1") { Id = 1 },
            new Tag("Tag2") { Id = 2 }
        };
        List<GetTagDto> mappedTags = new List<GetTagDto>
        {
            new GetTagDto { Id = 1, Name = "Tag1" },
            new GetTagDto { Id = 2, Name = "Tag2" }
        };

        _tagRepositoryMock.Setup( repo => repo.GetAll() ).ReturnsAsync( tags );
        _mapperMock.Setup( mapper => mapper.Map<IReadOnlyList<GetTagDto>>( tags ) ).Returns( mappedTags );

        // Act
        ResultT<IReadOnlyList<GetTagDto>> result = await _handler.Handle( new GetAllTagsQuery() );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.Equal( mappedTags, result.Value );
        _tagRepositoryMock.Verify( repo => repo.GetAll(), Times.Once );
        _mapperMock.Verify( mapper => mapper.Map<IReadOnlyList<GetTagDto>>( tags ), Times.Once );
    }

    [Fact]
    public async Task Handle_ReturnsEmptyList_WhenNoTagsExist()
    {
        // Arrange
        List<Tag> tags = new List<Tag>();
        List<GetTagDto> mappedTags = new List<GetTagDto>();

        _tagRepositoryMock.Setup( repo => repo.GetAll() ).ReturnsAsync( tags );
        _mapperMock.Setup( mapper => mapper.Map<IReadOnlyList<GetTagDto>>( tags ) ).Returns( mappedTags );

        // Act
        ResultT<IReadOnlyList<GetTagDto>> result = await _handler.Handle( new GetAllTagsQuery() );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.Empty( result.Value );
        _tagRepositoryMock.Verify( repo => repo.GetAll(), Times.Once );
        _mapperMock.Verify( mapper => mapper.Map<IReadOnlyList<GetTagDto>>( tags ), Times.Once );
    }
}

