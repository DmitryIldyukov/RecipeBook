using System.Security.Claims;

namespace Application.Common.ClaimInfo;

public static class ClaimInfo
{
    private static readonly string userId = "userId";

    public static int? GetUserId( this IEnumerable<Claim> claims )
    {
        if ( claims is null || !int.TryParse( claims.FirstOrDefault( c => c.Type == userId )?.Value, out int id ) )
        {
            return null;
        }

        return id;
    }
}
