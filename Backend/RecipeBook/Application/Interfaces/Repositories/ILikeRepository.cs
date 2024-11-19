using Application.Common.Repositories;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ILikeRepository : ICreateRepository<Like>, IDeleteRepository<Like>
{
    Task<Like> GetByUserIdAndRecipeId( int userId, int recipeId );
    Task<bool> IsRecipeLikedByUser( int userId, int recipeId );
}
