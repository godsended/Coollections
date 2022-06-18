using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Coollections.Services;

public class Auth : IAuth
{
    private readonly HttpContext httpContext;
    public Auth(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContext = httpContextAccessor.HttpContext!;
    }
    public async Task Authenticate(string email, bool isAdmin)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, email)
        };
        ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie",
            ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
        await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
    }
}