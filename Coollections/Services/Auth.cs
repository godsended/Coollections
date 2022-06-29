using System.Security.Claims;
using Coollections.Models.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Coollections.Services;

public class Auth : IAuth
{
    private readonly HttpContext httpContext;
    private readonly DatabaseContext databaseContext;

    public Auth(IHttpContextAccessor httpContextAccessor, DatabaseContext databaseContext)
    {
        this.databaseContext = databaseContext;
        this.httpContext = httpContextAccessor.HttpContext!;
    }

    public async Task Authenticate(string id, bool isAdmin)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, id)
        };
        ClaimsIdentity identity = new ClaimsIdentity(claims, "ApplicationCookie",
            ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
        await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
    }

    public async Task<bool> IsAuthorized()
    {
        int? id = GetUserId();
        if (id == null)
            return false;
        return await databaseContext.ContainsUserWithId(id.Value);
    }

    public int? GetUserId()
    {
        Claim? nameClaim = httpContext.User.Claims.FirstOrDefault
            (claim => claim.Type == ClaimsIdentity.DefaultNameClaimType);
        if (nameClaim == null) return null;
        int.TryParse(nameClaim.Value, out int result);
        {
            return result;
        }
    }
}