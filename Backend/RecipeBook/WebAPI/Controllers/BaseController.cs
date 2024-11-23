using Application.Common.ClaimInfo;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route( "api/[controller]" )]
public class BaseController : ControllerBase
{
    protected int? UserId => GetCurrentUserId();

    private int? GetCurrentUserId()
    {
        return User.Identity.IsAuthenticated
            ? ClaimInfo.GetUserId( HttpContext?.User.Claims )
            : null;
    }
}
