using System.Security.Claims;
using Coollections.Models;
using Coollections.Models.Database;
using Coollections.Models.Database.Items;
using Coollections.Services;
using Coollections.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coollections.Controllers;

public class CollectionsController : Controller
{
    private readonly DatabaseContext databaseContext;
    private readonly IAuth auth;

    public CollectionsController(DatabaseContext databaseContext, IAuth auth)
    {
        this.databaseContext = databaseContext;
        this.auth = auth;
    }

    [Authorize]
    [HttpGet]
    public IActionResult Create()
    {
        CreateCollectionViewModel viewModel = new() {Subjects = databaseContext.Subjects};
        return base.View(viewModel);
    }

    [HttpGet]
    [Route("[controller]/[action]/{id:int}")]
    public async Task<IActionResult> View(int id)
    {
        Collection? collection = await databaseContext.GetCollectionById(id);
        if (collection == null)
            return RedirectToAction("Index", "User");
        return base.View(await BuildCollectionViewModel(collection));
    }
    
    [HttpPost]
    [Route("[controller]/[action]")]
    public async Task<ResponseModel> AddItem(Dictionary<int, string> data)
    {
        if(!await auth.IsAuthorized())
            return new ResponseModel {IsSuccess = false, Message = "Not authorized", Code = 2};
        if (await databaseContext.AddDataRange(data))
            return new ResponseModel {IsSuccess = true, Message = "Item successfully saved", Code = 0};
        return new ResponseModel {IsSuccess = false, Message = "Incorrect data", Code = 1};
    }

    [HttpPost]
    public async Task<ResponseModel> Create(CreateCollectionRequestModel model)
    {
        if(!await auth.IsAuthorized())
            return new ResponseModel() {IsSuccess = false, Message = "Not authorized", Code = 2};
        await AddCollectionToDatabase(model);
        return new ResponseModel() {IsSuccess = true, Message = "Successfully created", Code = 0};
    }

    [HttpPost]
    [Route("[controller]/[action]/{item:int}/{collectionId:int}")]
    public async Task<ResponseModel> Delete(int item, int collectionId)
    {
        if(!await auth.IsAuthorized())
            return new ResponseModel() {IsSuccess = false, Message = "Not authorized", Code = 2};
        await databaseContext.RemoveItemFromCollection(item, collectionId);
        return new ResponseModel() {IsSuccess = true, Message = "Successfully deleted", Code = 0};
    }
    
    [HttpPost]
    [Route("[controller]/[action]/{item:int}/{collectionId:int}")]
    public async Task<ResponseModel> EditItem(Dictionary<int, string> data, int item, int collectionId)
    {
        if(!await auth.IsAuthorized())
            return new ResponseModel {IsSuccess = false, Message = "Not authorized", Code = 2};
        if (await databaseContext.UpdateDataRange(data, item, collectionId))
            return new ResponseModel {IsSuccess = true, Message = "Item successfully edited", Code = 0};
        return new ResponseModel {IsSuccess = false, Message = "Incorrect data", Code = 1};
    }
    private async Task AddCollectionToDatabase(CreateCollectionRequestModel model)
    {
        Collection collection = new Collection()
        {
            AuthorId = auth.GetUserId()!.Value,
            Description = model.Description,
            Image = model.ImageUrl,
            Name = model.Name,
            SubjectId = await databaseContext.SubjectIdByName(model.Subject)
        };
        int id = await databaseContext.AddCollection(collection);
        for (int i = 0; i < model.FieldsTypes!.Length; i++)
        {
            await databaseContext.AddField(new Field
            {
                CollectionId = id, 
                Name = model.FieldsNames![i],
                Type = model.FieldsTypes[i]
            });
        }
    }

    private async Task<CollectionViewModel> BuildCollectionViewModel(Collection collection)
    {
        IEnumerable<Field> fields = databaseContext.GetFieldsByCollectionId(collection.Id);
        List<Data> data = new List<Data>();
        var enumerable = fields.ToList();
        foreach (var field in enumerable)
            data.AddRange(databaseContext.GetDataByFieldId(field.Id).ToArray());
        
        return new CollectionViewModel()
        {
            Collection = collection,
            Data = data,
            Fields = enumerable,
            AuthorName = (await databaseContext.GetUserById(collection.AuthorId)).Name
        };
    }
}