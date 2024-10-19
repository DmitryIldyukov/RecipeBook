using System.Linq.Expressions;
using Application.Common.Repositories;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IUserRepository : ICreateRepository<User>
{
    Task<bool> ContainsAsync( Expression<Func<User, bool>> predicate );
}
