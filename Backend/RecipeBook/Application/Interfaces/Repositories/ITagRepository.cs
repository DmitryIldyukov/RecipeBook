using Application.Common.Repositories;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ITagRepository : ICreateRepository<Tag>
{
    Task<Tag> GetByName( string name );
    Task<IQueryable<Tag>> GetAll();
}
