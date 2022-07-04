using Coollections.Models.Database.Items;

namespace Coollections.ViewModels;

public class HomeViewModel
{
    public List<Collection> TopCollections { get; set; }
    public Dictionary<Collection, Dictionary<Field, Data>> LatestItems { get; set; }
}