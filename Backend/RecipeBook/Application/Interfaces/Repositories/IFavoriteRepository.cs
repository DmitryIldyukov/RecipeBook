using Application.Common.Repositories;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IFavoriteRepository : ICreateRepository<Favorite>, IDeleteRepository<Favorite>
{
    Task<Favorite> GetByUserIdAndRecipeId( int userId, int recipeId );
    Task<bool> IsUserFavoriteRecipe( int userId, int recipeId );
}
