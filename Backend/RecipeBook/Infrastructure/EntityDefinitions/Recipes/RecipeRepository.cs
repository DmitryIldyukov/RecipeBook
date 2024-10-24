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

    public async Task Delete( Recipe recipe )
    {
        dbContext.Recipes.Remove( recipe );
    }

    public IQueryable<Recipe> GetAll()
    {
        return dbContext.Recipes;
    }

    public async Task<Recipe> GetById( int id )
    {
        return await dbContext.Recipes.FirstOrDefaultAsync( r => r.Id == id );
    }
}
