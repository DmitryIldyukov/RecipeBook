using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityDefinitions.Steps;

public class StepRepository( RecipeBookDbContext dbContext ) : IStepRepository
{
    public async Task Create( Step step )
    {
        await dbContext.AddAsync( step );
    }

    public async Task Delete( Step entity )
    {
        dbContext.Steps.Remove( entity );
    }

    public async Task<IReadOnlyList<Step>> GetRecipeSteps( int recipeId )
    {
        return await dbContext.Steps.Where( s => s.RecipeId == recipeId ).ToListAsync();
    }
}
