using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityDefinitions.RefreshTokens;

public class RefreshTokenRepository( RecipeBookDbContext dbContext ) : IRefreshTokenRepository
{
    public async Task Create( RefreshToken refreshToken )
    {
        await dbContext.RefreshTokens.AddAsync( refreshToken );
    }

    public void Delete( RefreshToken refreshToken )
    {
        dbContext.RefreshTokens.Remove( refreshToken );
    }

    public async Task<RefreshToken> GetByToken( string token )
    {
        return await dbContext.RefreshTokens.FirstOrDefaultAsync( t => t.Token == token );
    }

    public async Task<RefreshToken> GetByUserId( int userId )
    {
        return await dbContext.RefreshTokens.FirstOrDefaultAsync( t => t.UserId == userId );
    }
}
