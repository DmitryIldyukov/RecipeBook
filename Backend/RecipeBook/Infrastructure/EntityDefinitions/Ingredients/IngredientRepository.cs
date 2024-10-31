using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityDefinitions.Ingredients;

public class IngredientRepository( RecipeBookDbContext dbContext ) : IIngredientRepository
{
    public async Task Create( Ingredient ingredient )
    {
        await dbContext.Ingredients.AddAsync( ingredient );
    }

    public void Delete( Ingredient ingredient )
    {
        dbContext.Ingredients.Remove( ingredient );
    }

    public async Task<Ingredient> GetById( int ingredientId )
    {
        return await dbContext.Ingredients.FirstOrDefaultAsync( i => i.Id == ingredientId );
    }

    public async Task<IReadOnlyList<Ingredient>> GetIngredientsByReceptId( int recipeId )
    {
        return await dbContext.Ingredients.Where( i => i.RecipeId == recipeId ).ToListAsync();
    }
}
