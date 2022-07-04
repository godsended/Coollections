using Coollections.Models;
using Coollections.Models.Database.Items;

namespace Coollections.Services;

public interface ICollectionsFilter
{
    Task<List<Collection>> GetTopCollections();
    Task<Dictionary<Collection, Dictionary<Field, Data>>> GetLatestItems();
}