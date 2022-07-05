using Coollections.Models.Database.Items;

namespace Coollections.ViewModels;

public class HomeViewModel : ViewModel
{
    public List<Collection> TopCollections { get; set; }
    public Dictionary<Collection, Dictionary<Field, Data>> LatestItems { get; set; }
}