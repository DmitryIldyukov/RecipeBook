using Application.Common.Result;
using Application.Interfaces.Repositories;
using Application.UseCases.Tags.Dtos;
using Application.UseCases.Tags.Queries.GetPopular;
using AutoMapper;
using Domain.Entities;
using Moq;

namespace Tests.Tags.Queries;

public class GetPopularTagsQueryHandlerTests
{
    private readonly Mock<ITagRepository> _tagRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetPopularTagsQueryValidator _validator;
    private readonly GetPopularTagsQueryHandler _handler;

    public GetPopularTagsQueryHandlerTests()
    {
        _tagRepositoryMock = new Mock<ITagRepository>();
        _mapperMock = new Mock<IMapper>();
        _validator = new GetPopularTagsQueryValidator();
        _handler = new GetPopularTagsQueryHandler( _tagRepositoryMock.Object, _validator, _mapperMock.Object );
    }

    [Fact]
    public async Task Handle_ValidQuery_ReturnsTags()
    {
        // Arrange
        GetPopularTagsQuery query = new GetPopularTagsQuery { Count = 3 };
        List<Tag> tags = new List<Tag>
        {
            new Tag("Tag1") { Id = 1 },
            new Tag("Tag2") { Id = 2 },
            new Tag("Tag3") { Id = 3 }
        };
        List<GetTagDto> tagDtos = tags.Select( t => new GetTagDto { Id = t.Id, Name = t.Name } ).ToList();

        _tagRepositoryMock.Setup( repo => repo.GetPopularTags( query.Count ) )
            .ReturnsAsync( tags );
        _mapperMock.Setup( mapper => mapper.Map<IReadOnlyList<GetTagDto>>( tags ) )
            .Returns( tagDtos );

        // Act
        ResultT<IReadOnlyList<GetTagDto>> result = await _handler.Handle( query );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.Equal( tagDtos, result.Value );
        _tagRepositoryMock.Verify( repo => repo.GetPopularTags( query.Count ), Times.Once );
        _mapperMock.Verify( mapper => mapper.Map<IReadOnlyList<GetTagDto>>( tags ), Times.Once );
    }

    [Fact]
    public async Task Handle_InvalidCount_Fail()
    {
        // Arrange
        GetPopularTagsQuery query = new GetPopularTagsQuery { Count = 0 };

        // Act
        ResultT<IReadOnlyList<GetTagDto>> result = await _handler.Handle( query );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Contains( "Запрашевоемое количество тегов должно быть больше 0.", result.ErrorMessages );
        _tagRepositoryMock.Verify( repo => repo.GetPopularTags( It.IsAny<int>() ), Times.Never );
    }
}

