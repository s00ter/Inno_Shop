using System.Security.Claims;
using Inno_Shop.BusinessLogic.Constants;
using Microsoft.AspNetCore.Http;

namespace Inno_Shop.BusinessLogic.Services.CurrentUserInfo;

public class CurrentUserInfo(IHttpContextAccessor httpContextAccessor) : ICurrentUserInfo
{
    public string GetUserId()
    {
        return httpContextAccessor?.HttpContext?.User.FindFirstValue(CustomClaimTypes.UserId) ?? throw new ArgumentException("Unable to retrieve user id");
    }
}