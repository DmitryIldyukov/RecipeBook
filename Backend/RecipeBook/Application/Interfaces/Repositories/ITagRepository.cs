using Application.Common.Repositories;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ITagRepository : ICreateRepository<Tag>, IDeleteRepository<Tag>
{
    Task<Tag> GetByName( string name );
    Task<IReadOnlyList<Tag>> GetAll();
    Task<bool> IsUsedInOtherRecipes( int tagId, int RecipeId );
}
