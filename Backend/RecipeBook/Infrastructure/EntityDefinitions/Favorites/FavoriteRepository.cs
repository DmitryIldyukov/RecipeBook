using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityDefinitions.Favorites;

public class FavoriteRepository( RecipeBookDbContext dbContext ) : IFavoriteRepository
{
    public async Task Create( Favorite favorite )
    {
        await dbContext.AddAsync( favorite );
    }

    public void Delete( Favorite favorite )
    {
        dbContext.Remove( favorite );
    }

    public async Task<Favorite> GetByUserAndRecipeId( int userId, int recipeId )
    {
        return await dbContext.Favorites
            .FirstOrDefaultAsync( f => f.UserId == userId && f.RecipeId == recipeId );
    }

    public async Task<bool> UserHasRecipeInFavorites( int userId, int recipeId )
    {
        return await dbContext.Favorites.AnyAsync( f => f.UserId == userId && f.RecipeId == recipeId );
    }
}
