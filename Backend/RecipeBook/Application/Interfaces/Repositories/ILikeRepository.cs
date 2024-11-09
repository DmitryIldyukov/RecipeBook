using Application.Common.Repositories;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ILikeRepository : ICreateRepository<Like>, IDeleteRepository<Like>
{
    Task<Like> GetByUserAndRecipeId( int userId, int recipeId );
    Task<bool> UserHasRecipeInLikes( int userId, int recipeId );
}
