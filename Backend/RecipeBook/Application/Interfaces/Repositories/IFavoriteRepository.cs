using Application.Common.Repositories;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IFavoriteRepository : ICreateRepository<Favorite>, IDeleteRepository<Favorite>
{
    Task<Favorite> GetById( int id );
    Task<bool> UserHasRecipeInFavorites( int userId, int recipeId );
}
