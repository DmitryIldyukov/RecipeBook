using Domain.Entities;

namespace Application.Common.Repositories;

public interface IDeleteRepository<T> where T : Entity
{
    void Delete( T entity );
}
