using System.Security.Claims;

namespace ResourcesManagementApi.Controllers;

public static class HttpContextAccessorExtensions
{
    public static int? ParseUserId(this IHttpContextAccessor httpContextAccessor)
    {
        var claims = httpContextAccessor.HttpContext.User.FindFirstValue("UserId");

        if (int.TryParse(claims, out int userId))
        {
            return userId;
        }

        return null;
    }
}
