using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Coollections.Models;
using Microsoft.AspNetCore.Authorization;

namespace Coollections.Controllers;

public class HomeController : Controller
{
    public HomeController()
    {
    }

    [HttpGet]
    [Authorize]
    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}