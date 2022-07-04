using System.Security.Claims;
using Coollections.Models.Database;
using Coollections.Models.Database.Items;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

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
        if (!await databaseContext.ContainsUserWithId(id.Value))
            return false;
        return !(await databaseContext.Users.FirstOrDefaultAsync(u => u.Id == id.Value))!.IsBlocked;
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

    public async Task<bool> HasAccessToCollection(int collectionId)
    {
        Collection? collection = await databaseContext.GetCollectionById(collectionId);
        return collection != null && await IsAuthorized() && (collection.AuthorId == GetUserId() 
                                      || await databaseContext.IsAdmin(GetUserId()!.Value));
    }
}