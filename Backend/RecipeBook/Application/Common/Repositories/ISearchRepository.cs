using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Common.Repositories;

public interface ISearchRepository<T> where T : Entity
{
    Task<bool> ContainsAsync( Expression<Func<T, bool>> predicate );
}
