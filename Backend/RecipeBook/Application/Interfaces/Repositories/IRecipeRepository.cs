using Application.Common.Repositories;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IRecipeRepository : ICreateRepository<Recipe>, IDeleteRepository<Recipe>
{
    Task<Recipe> GetById( int id );
    Task<Recipe> GetDailyRecipe();
}
