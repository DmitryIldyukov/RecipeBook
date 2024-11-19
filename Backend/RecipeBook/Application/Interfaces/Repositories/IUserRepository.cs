using Application.Common.Repositories;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IUserRepository : ICreateRepository<User>, ISearchRepository<User>
{
    Task<User> GetById( int id );
    Task<User> GetByLogin( string login );
}
