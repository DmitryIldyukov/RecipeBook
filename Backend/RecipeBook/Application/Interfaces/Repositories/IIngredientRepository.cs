using Application.Common.Repositories;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IIngredientRepository : ICreateRepository<Ingredient>, IDeleteRepository<Ingredient>
{
    Task<IReadOnlyList<Ingredient>> GetIngredientsByReceptId( int recipeId );
}
