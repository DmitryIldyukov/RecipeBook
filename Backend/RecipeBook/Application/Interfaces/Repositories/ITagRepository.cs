using Application.Common.Repositories;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ITagRepository : ICreateRepository<Tag>, IDeleteRepository<Tag>
{
    Task<Tag> GetByName( string name );
    Task<IReadOnlyList<Tag>> GetAll();
    Task<IReadOnlyList<Tag>> GetPopularTags( int count );
    Task<bool> IsUsedInMultipleRecipes( int tagId );
}
