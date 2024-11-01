using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityDefinitions.Likes;

public class LikeRepository( RecipeBookDbContext dbContext ) : ILikeRepository
{
    public async Task Create( Like Like )
    {
        await dbContext.AddAsync( Like );
    }

    public void Delete( Like Like )
    {
        dbContext.Remove( Like );
    }

    public async Task<Like> GetById( int id )
    {
        return await dbContext.Likes.FirstOrDefaultAsync( f => f.Id == id );
    }

    public async Task<bool> UserHasRecipeInLikes( int userId, int recipeId )
    {
        return await dbContext.Likes.AnyAsync( f => f.UserId == userId && f.RecipeId == recipeId );
    }
}
