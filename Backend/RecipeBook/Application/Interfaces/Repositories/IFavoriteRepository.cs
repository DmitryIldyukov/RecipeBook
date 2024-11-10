using Application.Common.Repositories;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IFavoriteRepository : ICreateRepository<Favorite>, IDeleteRepository<Favorite>
{
    Task<Favorite> GetByUserAndRecipeId( int userId, int recipeId );
    Task<bool> UserHasRecipeInFavorites( int userId, int recipeId );
}
