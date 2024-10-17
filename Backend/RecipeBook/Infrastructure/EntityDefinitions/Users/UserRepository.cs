using System.Linq.Expressions;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityDefinitions.Users;

public class UserRepository( RecipeBookDbContext dbContext ) : IUserRepository
{
    public async Task Create( User user )
    {
        await dbContext.AddAsync( user );
    }

    public async Task<bool> ContainsAsync( Expression<Func<User, bool>> predicate )
    {
        return await dbContext.Users.AnyAsync( predicate );
    }
}
