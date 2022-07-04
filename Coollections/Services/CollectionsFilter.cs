using Coollections.Models;
using Coollections.Models.Database;
using Coollections.Models.Database.Items;
using Microsoft.EntityFrameworkCore;

namespace Coollections.Services;

public class CollectionsFilter : ICollectionsFilter
{
    private readonly DatabaseContext databaseContext;
    
    public CollectionsFilter(DatabaseContext databaseContext)
    {
        this.databaseContext = databaseContext;
    }
    
    public async Task<List<Collection>> GetTopCollections()
    {
        return await databaseContext.Collections.OrderByDescending(c => c.ItemsCount).Take(5).ToListAsync();
    }
    
    public async Task<Dictionary<Collection, Dictionary<Field, Data>>> GetLatestItems()
    {
        Dictionary<Collection, Dictionary<Field, Data>> res = new();
        List<Item> items = await databaseContext.Items.OrderByDescending(i => i.Id).Take(5).ToListAsync();
        foreach (var item in items)
        {
            List<Data> data = await databaseContext.GetDataByItemId(item.Id);
            Dictionary<Field, Data> dataByField = new();
            foreach (var d in data)
            {
                dataByField.Add((await databaseContext.GetFieldById(d.FieldId))!, d);
            }

            Collection collection = (await databaseContext.GetCollectionById(item.CollectionId))!;
            res.Add(collection, dataByField);
        }

        return res;
    }
}