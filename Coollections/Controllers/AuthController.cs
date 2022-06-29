using Coollections.Models;
using Coollections.Models.Database;
using Coollections.Models.Database.Items;
using Coollections.Services;
using Microsoft.AspNetCore.Mvc;

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
            if (!await databaseContext.IsLoginDataCorrect(viewData.Email, viewData.Password))
                return new ResponseModel() {IsSuccess = false, Message = "Incorrect login data" , Code = 2};

            await auth.Authenticate((await databaseContext.GetUserIdByEmail(viewData.Email)).ToString(), 
                false).ConfigureAwait(false);
        }
        else return new ResponseModel() {IsSuccess = false, Message = "Invalid request data", Code = 1};
        
        return new ResponseModel() {IsSuccess = true, Message = "Successfully login" , Code = 0};
    }

    [HttpPost]
    public async Task<ResponseModel> ProcessRegistration(User viewData)
    {
        if (ModelState.IsValid && viewData.IsValid())
        {
            if (await databaseContext.ContainsUserWithEmail(viewData.Email))
                return new ResponseModel() {IsSuccess = false, Message = "User already registered" , Code = 2};
            
            int id = await databaseContext.AddUser(viewData);
            await auth.Authenticate(id.ToString(), false);
        }
        else return new ResponseModel() {IsSuccess = false, Message = "Invalid request data" , Code = 1};
        
        return new ResponseModel() {IsSuccess = true, Message = "Successfully registered" , Code = 0};
    }
}