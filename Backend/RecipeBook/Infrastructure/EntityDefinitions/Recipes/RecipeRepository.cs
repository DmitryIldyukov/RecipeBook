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
            .OrderByDescending( r => r.Likes.Where( l => l.CreatedAt > DateTime.Now.AddDays( -1 ) ).Count() )
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<Recipe>> GetUserFavoriteRecipesByPage( int userId, Page page )
    {
        IQueryable<Recipe> recipesQuery = dbContext.Recipes
            .Include( r => r.Ingredients )
            .Include( r => r.Steps )
            .Include( r => r.Tags )
            .Include( r => r.Likes )
            .Include( r => r.Favorites )
            .Include( r => r.Author )
            .Where( r => r.Favorites.Any( f => f.UserId == userId ) );

        recipesQuery = recipesQuery
            .Skip( ( page.PageNumber - 1 ) * page.PageSize )
            .Take( page.PageSize );

        return await recipesQuery.ToListAsync();
    }

    public async Task<bool> ContainsAsync( Expression<Func<Recipe, bool>> predicate )
    {
        return await dbContext.Recipes.AnyAsync( predicate );
    }

    public async Task<IReadOnlyList<Recipe>> GetRecipesByFilter( string searchString, Page page )
    {
        IQueryable<Recipe> recipes = dbContext.Recipes
            .Include( r => r.Tags )
            .Include( r => r.Likes )
            .Include( r => r.Favorites )
            .Include( r => r.Author );

        searchString = searchString.Trim();

        if ( !string.IsNullOrEmpty( searchString ) )
        {
            recipes = recipes.Where( r =>
                EF.Functions.Like( r.Name, $"%{searchString}%" ) ||
                r.Tags.Any( t => EF.Functions.Like( t.Name, $"%{searchString}%" ) )
            );
        }

        recipes = recipes
            .Skip( ( page.PageNumber - 1 ) * page.PageSize )
            .Take( page.PageSize );

        return await recipes.ToListAsync();
    }
}
