using System.Security.Claims;
using Coollections.Models.Database;
using Coollections.Models.Database.Items;
using Coollections.Services;
using Coollections.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coollections.Controllers;

public class UserController : Controller
{
    private readonly DatabaseContext databaseContext;
    private readonly IAuth auth;

    public UserController(DatabaseContext databaseContext, IAuth auth)
    {
        this.databaseContext = databaseContext;
        this.auth = auth;
    }
    
    [HttpGet]
    [Route("[controller]/[action]/{id?}")]
    public async Task<IActionResult> Index(int? id)
    {
        if (id != null)
            return await ProcessIdIndexRequest(id.Value);
        else
        {
            if (!await auth.IsAuthorized())
                return RedirectToAction("Login", "Auth");
            return await ProcessIdIndexRequest(auth.GetUserId()!.Value);
        }
    }

    private async Task<IActionResult> ProcessIdIndexRequest(int id)
    {
        
        User? user = await databaseContext.Users.FirstOrDefaultAsync(u => u.Id == id).ConfigureAwait(false);
        if (user != null)
        {
            IEnumerable<Collection> collections = databaseContext.GetCollectionsByAuthorId(id);
            return View(new UserViewModel() {User = user, Collections = collections});
        }
        else return RedirectToAction("Index", "Home");
    }
}