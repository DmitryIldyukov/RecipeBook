using Application.Common.Repositories;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ITagRepository : ICreateRepository<Tag>
{
    Task<Tag> GetByName( string name );
    IQueryable<Tag> GetAll();
}
