using System.Diagnostics;
using Coollections.Models.Database.Items;
using Coollections.Services;
using Microsoft.AspNetCore.Mvc;
using Coollections.ViewModels;

namespace Coollections.Controllers;

public class HomeController : Controller
{
    private readonly ICollectionsFilter collectionsFilter;

    public HomeController(ICollectionsFilter collectionsFilter)
    {
        this.collectionsFilter = collectionsFilter;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        List<Collection> topCollections = await collectionsFilter.GetTopCollections();
        Dictionary<Collection, Dictionary<Field, Data>> items = await collectionsFilter.GetLatestItems();
        HomeViewModel viewModel = new HomeViewModel {LatestItems = items, TopCollections = topCollections};
        return View(viewModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}