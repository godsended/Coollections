using Coollections.Models;
using Coollections.Models.Database;
using Coollections.Models.Database.Items;
using Coollections.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coollections.Controllers;

public class AuthController : Controller
{
    private readonly DatabaseContext databaseContext;
    private readonly IAuth auth;

    public AuthController(DatabaseContext databaseContext, IAuth auth)
    {
        this.databaseContext = databaseContext;
        this.auth = auth;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Registration()
    {
        return View();
    }

    [HttpPost]
    public async Task<ResponseModel> ProcessLogin(User viewData)
    {
        if (ModelState.IsValid && viewData.IsValidForLogin())
        {
            if (!await databaseContext.IsLoginDataCorrect(viewData.Email, viewData.Password).ConfigureAwait(false))
                return new ResponseModel() {IsSuccess = false, Message = "Incorrect login data" , Code = 2};

            await auth.Authenticate(viewData.Email, false).ConfigureAwait(false);
        }
        else return new ResponseModel() {IsSuccess = false, Message = "Invalid request data", Code = 1};
        
        return new ResponseModel() {IsSuccess = true, Message = "Successfully login" , Code = 0};
    }

    [HttpPost]
    public async Task<ResponseModel> ProcessRegistration(User viewData)
    {
        if (ModelState.IsValid && viewData.IsValid())
        {
            if (await databaseContext.ContainsUserWithEmail(viewData.Email).ConfigureAwait(false))
                return new ResponseModel() {IsSuccess = false, Message = "User already registered" , Code = 2};
            
            await databaseContext.AddUser(viewData).ConfigureAwait(false);
            await auth.Authenticate(viewData.Email, false).ConfigureAwait(false);
        }
        else return new ResponseModel() {IsSuccess = false, Message = "Invalid request data" , Code = 1};
        
        return new ResponseModel() {IsSuccess = true, Message = "Successfully registered" , Code = 0};
    }
}