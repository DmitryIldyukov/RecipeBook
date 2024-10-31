using Application.Common.Repositories;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IStepRepository : ICreateRepository<Step>, IDeleteRepository<Step>
{
    Task<IReadOnlyList<Step>> GetRecipeSteps( int recipeId );
    Task<Step> GetById( int stepId );
}
