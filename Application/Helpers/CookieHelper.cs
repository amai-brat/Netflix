using Microsoft.AspNetCore.Http;

namespace Application.Helpers;

public static class CookieHelper
{
    public static void AddRefreshTokenCookie(HttpContext httpContext, string refreshToken)
    {
        httpContext.Response.Cookies.Append("refresh-token", 
            refreshToken, 
            new CookieOptions
            {
                SameSite = SameSiteMode.None,
                HttpOnly = true,
                Secure = true,
                MaxAge = TimeSpan.FromDays(30)
            });
    }
}