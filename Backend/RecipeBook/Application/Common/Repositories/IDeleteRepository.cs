using Domain.Entities;

namespace Application.Common.Repositories;

public interface IDeleteRepository<T> where T : Entity
{
    Task Delete( T entity );
}
