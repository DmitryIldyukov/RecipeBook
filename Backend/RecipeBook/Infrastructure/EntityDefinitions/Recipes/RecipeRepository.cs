using System.Linq.Expressions;
using Application.Common.Page;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityDefinitions.Recipes;

public class RecipeRepository( RecipeBookDbContext dbContext ) : IRecipeRepository
{
    public async Task Create( Recipe recipe )
    {
        await dbContext.Recipes.AddAsync( recipe );
    }

    public void Delete( Recipe recipe )
    {
        dbContext.Recipes.Remove( recipe );
    }

    public async Task<Recipe> GetById( int id )
    {
        return await dbContext.Recipes
            .Include( r => r.Ingredients )
            .Include( r => r.Steps )
            .Include( r => r.Tags )
            .Include( r => r.Likes )
            .Include( r => r.Favorites )
            .Include( r => r.Author )
            .FirstOrDefaultAsync( r => r.Id == id );
    }

    public async Task<Recipe> GetDailyRecipe()
    {
        return await dbContext.Recipes
            .Include( r => r.Likes )
            .OrderBy( r => r.Likes.Where( l => l.CreatedAt > DateTime.Now.AddDays( -1 ) ).Count() )
            .FirstOrDefaultAsync();
    }

    public IQueryable<Recipe> GetUserFavoriteRecipesByPage( int userId, Page page )
    {
        return dbContext.Recipes
            .Include( r => r.Ingredients )
            .Include( r => r.Steps )
            .Include( r => r.Tags )
            .Include( r => r.Likes )
            .Include( r => r.Favorites )
            .Include( r => r.Author )
            .Where( r => r.Favorites.Any( f => f.UserId == userId ) )
            .Skip( ( page.PageNumber ) * page.PageSize )
            .Take( page.PageSize );
    }

    public async Task<bool> ContainsAsync( Expression<Func<Recipe, bool>> predicate )
    {
        return await dbContext.Recipes.AnyAsync( predicate );
    }
}
