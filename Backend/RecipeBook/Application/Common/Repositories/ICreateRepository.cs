using Domain.Entities;

namespace Application.Common.Repositories;

public interface ICreateRepository<T> where T : Entity
{
    Task Create( T entity );
}
