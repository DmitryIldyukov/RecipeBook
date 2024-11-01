using System.Linq.Expressions;
using Application.Common.Page;
using Application.Common.Repositories;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IRecipeRepository : ICreateRepository<Recipe>, IDeleteRepository<Recipe>
{
    Task<Recipe> GetById( int id );
    Task<Recipe> GetDailyRecipe();
    IQueryable<Recipe> GetUserFavoriteRecipesByPage( int userId, Page page );
    Task<bool> ContainsAsync( Expression<Func<Recipe, bool>> predicate );
}
