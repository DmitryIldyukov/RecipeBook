using Application.Common.Page;
using Application.Common.Repositories;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IRecipeRepository : ICreateRepository<Recipe>, IDeleteRepository<Recipe>, ISearchRepository<Recipe>
{
    Task<Recipe> GetById( int id );
    Task<Recipe> GetDailyRecipe();
    Task<IReadOnlyList<Recipe>> GetUserFavoriteRecipesByPage( int userId, Page page );
    Task<IReadOnlyList<Recipe>> GetRecipesByFilter( List<string> searchQuery, Page page );
    Task<IReadOnlyList<Recipe>> GetUserRecipes( int userId );
}
