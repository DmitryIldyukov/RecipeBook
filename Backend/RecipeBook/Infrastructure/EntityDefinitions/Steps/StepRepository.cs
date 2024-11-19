using System.Linq.Expressions;
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

    public void Delete( Step entity )
    {
        dbContext.Steps.Remove( entity );
    }

    public async Task<IReadOnlyList<Step>> GetStepsByRecipeId( int recipeId )
    {
        return await dbContext.Steps.Where( s => s.RecipeId == recipeId ).ToListAsync();
    }

    public async Task<Step> GetById( int stepId )
    {
        return await dbContext.Steps.FirstOrDefaultAsync( s => s.Id == stepId );
    }

    public async Task<bool> ContainsAsync( Expression<Func<Step, bool>> predicate )
    {
        return await dbContext.Steps.AnyAsync( predicate );
    }
}
