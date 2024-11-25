using Application.Common.Repositories;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IRefreshTokenRepository : ICreateRepository<RefreshToken>, IDeleteRepository<RefreshToken>
{
    Task<RefreshToken> GetByToken( string token );
    Task<RefreshToken> GetByUserId( int userId );
}
