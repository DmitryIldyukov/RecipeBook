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

    public async Task Delete( Ingredient ingredient )
    {
        dbContext.Ingredients.Remove( ingredient );
    }

    public async Task<IReadOnlyList<Ingredient>> GetRecipeIngredients( int recipeId )
    {
        return await dbContext.Ingredients.Where( i => i.RecipeId == recipeId ).ToListAsync();
    }
}
