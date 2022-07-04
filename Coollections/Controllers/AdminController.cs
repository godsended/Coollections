using Coollections.Models;
using Coollections.Models.Database;
using Coollections.Services;
using Coollections.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Coollections.Controllers;

public class AdminController : Controller
{
    private readonly DatabaseContext databaseContext;
    private readonly IAuth auth;

    public AdminController(DatabaseContext databaseContext, IAuth auth)
    {
        this.databaseContext = databaseContext;
        this.auth = auth;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (!await auth.IsAuthorized() || !await databaseContext.IsAdmin(auth.GetUserId()!.Value))
            return RedirectToAction("Index", "Home");
        return View(new AdminViewModel {Users = databaseContext.Users});
    }

    [HttpPost]
    public async Task<ResponseModel> BlockUsers(AdminRequestModel? requestModel)
    {
        if(!await auth.IsAuthorized() || !await databaseContext.IsAdmin(auth.GetUserId()!.Value))
            return new ResponseModel {IsSuccess = false, Message = "Not authorized", Code = 2};
        if (requestModel?.Ids == null)
            return new ResponseModel {IsSuccess = false, Message = "Incorrect data", Code = 1};
        foreach (var id in requestModel.Ids)
        {
            await databaseContext.SetBlock(true, id);
        }
        return new ResponseModel {IsSuccess = true, Message = "Users successfully blocked", Code = 0};
    }
    
    [HttpPost]
    public async Task<ResponseModel> UnlockUsers(AdminRequestModel? requestModel)
    {
        if(!await auth.IsAuthorized() || !await databaseContext.IsAdmin(auth.GetUserId()!.Value))
            return new ResponseModel {IsSuccess = false, Message = "Not authorized", Code = 2};
        if (requestModel?.Ids == null)
            return new ResponseModel {IsSuccess = false, Message = "Incorrect data", Code = 1};
        foreach (var id in requestModel.Ids)
        {
            await databaseContext.SetBlock(false, id);
        }
        return new ResponseModel {IsSuccess = true, Message = "Users successfully unblocked", Code = 0};
    }
    
    [HttpPost]
    public async Task<ResponseModel> AddAdmins(AdminRequestModel? requestModel)
    {
        if(!await auth.IsAuthorized() || !await databaseContext.IsAdmin(auth.GetUserId()!.Value))
            return new ResponseModel {IsSuccess = false, Message = "Not authorized", Code = 2};
        if (requestModel?.Ids == null)
            return new ResponseModel {IsSuccess = false, Message = "Incorrect data", Code = 1};
        foreach (var id in requestModel.Ids)
        {
            await databaseContext.SetAdminByUserId(true, id);
        }
        return new ResponseModel {IsSuccess = true, Message = "Users successfully added", Code = 0};
    }
    
    [HttpPost]
    public async Task<ResponseModel> RemoveAdmins(AdminRequestModel? requestModel)
    {
        if(!await auth.IsAuthorized() || !await databaseContext.IsAdmin(auth.GetUserId()!.Value))
            return new ResponseModel {IsSuccess = false, Message = "Not authorized", Code = 2};
        if (requestModel?.Ids == null)
            return new ResponseModel {IsSuccess = false, Message = "Incorrect data", Code = 1};
        foreach (var id in requestModel.Ids)
        {
            await databaseContext.SetAdminByUserId(false, id);
        }
        return new ResponseModel {IsSuccess = true, Message = "Users successfully removed", Code = 0};
    }
    
    [HttpPost]
    public async Task<ResponseModel> DeleteUsers(AdminRequestModel? requestModel)
    {
        if(!await auth.IsAuthorized() || !await databaseContext.IsAdmin(auth.GetUserId()!.Value))
            return new ResponseModel {IsSuccess = false, Message = "Not authorized", Code = 2};
        if (requestModel?.Ids == null)
            return new ResponseModel {IsSuccess = false, Message = "Incorrect data", Code = 1};
        foreach (var id in requestModel.Ids)
        {
            await databaseContext.DeleteUser(id);
        }
        return new ResponseModel {IsSuccess = true, Message = "Users successfully deleted", Code = 0};
    }
}