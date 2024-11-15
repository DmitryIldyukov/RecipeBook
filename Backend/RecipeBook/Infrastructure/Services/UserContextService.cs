using System.Security.Claims;
using Application.Common.ClaimInfo;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class UserContextService( IHttpContextAccessor httpContextAccessor ) : IUserContextService
{
    public int? GetCurrentUserId()
    {
        IEnumerable<Claim> claims = httpContextAccessor.HttpContext?.User.Claims;
        return ClaimInfo.GetUserId( claims );
    }
}
