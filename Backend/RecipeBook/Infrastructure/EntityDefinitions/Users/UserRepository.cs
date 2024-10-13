using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityDefinitions.Users;

public class UserRepository( RecipeBookDbContext context ) : IUserRepository
{
    public async Task Create( User user )
    {
        await context.AddAsync( user );
    }

    public async Task<bool> UserIsExist( string login )
    {
        return await context.Users.AnyAsync( user => user.Login == login );
    }
}
