using System.Linq.Expressions;
using Application.Common.Page;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            .Include( r => r.Author )
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
            .Take( page.PageSize + 1 );

        return await recipesQuery.ToListAsync();
    }

    public async Task<bool> ContainsAsync( Expression<Func<Recipe, bool>> predicate )
    {
        return await dbContext.Recipes.AnyAsync( predicate );
    }

    public async Task<IReadOnlyList<Recipe>> GetRecipesByFilter( List<string> searchQueries, Page page )
    {
        IQueryable<Recipe> recipes = dbContext.Recipes
            .Include( r => r.Tags )
            .Include( r => r.Likes )
            .Include( r => r.Favorites )
            .Include( r => r.Author );

        if ( searchQueries is not null && searchQueries.Any() )
        {
            List<string> trimmedQuery = searchQueries.Select( s => s.ToLower().Trim() ).ToList();

            recipes = recipes.Where( r => trimmedQuery.Any( q =>
                r.Name.Contains( q ) ) ||
                trimmedQuery.Any( q => r.Tags.Any( t => t.Name.Contains( q ) ) ) ).AsQueryable();
        }

        recipes = recipes
            .Skip( ( page.PageNumber - 1 ) * page.PageSize )
            .Take( page.PageSize + 1 );

        return await recipes.ToListAsync();
    }

    public async Task<IReadOnlyList<Recipe>> GetUserRecipes( int userId )
    {
        return await dbContext.Recipes
           .Include( r => r.Ingredients )
           .Include( r => r.Steps )
           .Include( r => r.Tags )
           .Include( r => r.Likes )
           .Include( r => r.Favorites )
           .Include( r => r.Author )
           .Where( r => r.AuthorId == userId )
           .ToListAsync();
    }
}
