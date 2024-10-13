using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task Create( User user );
    Task<bool> UserIsExist( string login );
}
